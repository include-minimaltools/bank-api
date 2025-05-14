using Xunit;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankApi.Application.Commands.CreateTransaction;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;
using BankApi.Domain.Interfaces.Repositories;
using FluentAssertions;

namespace BankApi.Application.Tests;

public class CreateTransactionUseCaseHandlerTests
{
    private readonly Mock<IBankAccountRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IIdGenerator> _idGenMock;
    private readonly CreateTransactionUseCaseHandler _handler;

    public CreateTransactionUseCaseHandlerTests()
    {
        _repoMock = new Mock<IBankAccountRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _idGenMock = new Mock<IIdGenerator>();

        _uowMock.Setup(u => u.BankAccountRepository).Returns(_repoMock.Object);
        _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _handler = new CreateTransactionUseCaseHandler(_uowMock.Object, _idGenMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Deposit_Amount_When_Type_Is_D()
    {
        // Arrange
        var account = new BankAccount { Id = 1, Balance = 1000 };
        var request = new CreateTransactionUseCaseCommand
        {
            Id = 1,
            TransactionType = "D",
            Amount = 500
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(account);
        _idGenMock.Setup(g => g.NewId()).Returns(99L);

        var result = await _handler.HandleAsync(request);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(99L, result.Value);
        Assert.Equal(1500, account.Balance);
        Assert.Single(account.BankTransactions);
        Assert.Equal(1500, account.BankTransactions.First().Balance);
    }

    [Fact]
    public async Task HandleAsync_Should_Withdraw_Amount_When_Type_Is_W()
    {
        var account = new BankAccount { Id = 1, Balance = 1000 };
        var request = new CreateTransactionUseCaseCommand
        {
            Id = 1,
            TransactionType = "W",
            Amount = 400
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(account);
        _idGenMock.Setup(g => g.NewId()).Returns(55L);

        var result = await _handler.HandleAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(55L, result.Value);
        Assert.Equal(600, account.Balance);
        Assert.Single(account.BankTransactions);
        Assert.Equal(600, account.BankTransactions.First().Balance);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_NotFound_If_Account_Does_Not_Exist()
    {
        var request = new CreateTransactionUseCaseCommand
        {
            Id = 99,
            TransactionType = "D",
            Amount = 500
        };

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((BankAccount?)null);

        var result = await _handler.HandleAsync(request);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_ValidationError_If_Amount_Is_Negative()
    {
        var account = new BankAccount { Id = 1, Balance = 500 };
        var request = new CreateTransactionUseCaseCommand
        {
            Id = 1,
            TransactionType = "D",
            Amount = -50
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(account);

        var result = await _handler.HandleAsync(request);

        Assert.True(result.IsFailure);
        Assert.NotNull(result.ErrorDetails);
        Assert.Contains(result.ErrorDetails, e => e.Key == "amount");
    }

    [Fact]
    public async Task HandleAsync_Should_Return_ValidationError_If_Withdrawal_Exceeds_Balance()
    {
        var account = new BankAccount { Id = 1, Balance = 100 };
        var request = new CreateTransactionUseCaseCommand
        {
            Id = 1,
            TransactionType = "W",
            Amount = 200
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(account);

        var result = await _handler.HandleAsync(request);

        Assert.True(result.IsFailure);
        Assert.NotNull(result.ErrorDetails);
        Assert.Contains(result.ErrorDetails, e => e.Key == "amount");
    }
}
