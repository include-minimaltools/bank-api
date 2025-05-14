using AutoMapper;
using BankApi.Application.DTOs;
using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Services;

public class CustomerService(IUnitOfWork uow, IMapper mapper, IIdGenerator idGenerator) : ICustomerService
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;
    private readonly IIdGenerator _idGenerator = idGenerator;

    public async Task<Result<long>> CreateAsync(CustomerRequestDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Customer>(dto);

        entity.Id = _idGenerator.NewId();
        entity.CreatedAt = DateTime.UtcNow;

        await _uow.CustomerRepository.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Result<IEnumerable<CustomerDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Customer> entities = await _uow.CustomerRepository.GetAllAsync(cancellationToken);

        IEnumerable<CustomerDto> dtos = _mapper.Map<IEnumerable<CustomerDto>>(entities);

        return Result.Success(dtos);
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        Customer? entity = await _uow.CustomerRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Result<CustomerDto>.NotFound("El cliente no existe en el sistema");

        CustomerDto dto = _mapper.Map<CustomerDto>(entity);

        return Result.Success(dto);
    }

    public async Task<Result<Unit>> HardDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        Customer? entity = await _uow.CustomerRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Result<Unit>.NotFound("El cliente no existe en el sistema");

        _uow.CustomerRepository.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<Unit>> UpdateAsync(long id, CustomerRequestDto dto, CancellationToken cancellationToken = default)
    {
        Customer? entity = await _uow.CustomerRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            return Result<Unit>.NotFound("El cliente no existe en el sistema");

        _mapper.Map(dto, entity);

        entity.UpdatedAt = DateTime.UtcNow;

        await _uow.CustomerRepository.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}