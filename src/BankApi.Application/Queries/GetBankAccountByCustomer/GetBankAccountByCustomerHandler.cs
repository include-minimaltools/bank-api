using AutoMapper;
using BankApi.Application.DTOs;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Queries.GetBankAccountByCustomer;

public class GetBankAccountByCustomerHandler(IUnitOfWork unitOfWork, IMapper mapper) : IUseCaseHandler<GetBankAccountByCustomerCommand, IEnumerable<BankAccountDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<BankAccountDto>>> HandleAsync(GetBankAccountByCustomerCommand request, CancellationToken cancellationToken = default)
    {
        var existCustomer = await _unitOfWork.CustomerRepository.AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (!existCustomer)
            return Result<IEnumerable<BankAccountDto>>.NotFound("No se ha encontrado el cliente en el sistema");

        var entities = await _unitOfWork.BankAccountRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        IEnumerable<BankAccountDto> dtos = _mapper.Map<IEnumerable<BankAccountDto>>(entities);

        return Result.Success(dtos);
    }
}