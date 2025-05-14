using Xunit;
using Moq;
using AutoMapper;
using BankApi.Application.Services;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Application.DTOs;
using FluentAssertions;

namespace BankApi.Application.Tests;

public class CustomerServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIdGenerator> _idGenMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _idGenMock = new Mock<IIdGenerator>();

        _service = new CustomerService(_uowMock.Object, _mapperMock.Object, _idGenMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Map_And_Save_Customer()
    {
        var dto = new CustomerRequestDto { FirstName = "Gabriel" };
        var customer = new Customer { FirstName = "Gabriel", BirthDate = DateTime.Now, CreatedAt = DateTime.Now, Gender = "M", Income = 1000 };
        long generatedId = 123456;

        _mapperMock.Setup(m => m.Map<Customer>(dto)).Returns(customer);
        _idGenMock.Setup(g => g.NewId()).Returns(generatedId);

        _uowMock.Setup(u => u.CustomerRepository.AddAsync(customer, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()));

        var result = await _service.CreateAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(generatedId);
        customer.Id.Should().Be(generatedId);
        customer.CreatedAt.Should().NotBe(default);
        _uowMock.Verify(u => u.CustomerRepository.AddAsync(customer, It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
