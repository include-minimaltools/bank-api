using AutoMapper;
using BankApi.Application.DTOs;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Queries.GetTransactionsByBankAccount;

public class GetTransactionsByBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) : IUseCaseHandler<GetTransactionsByBankAccountCommand, IEnumerable<BankTransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<BankTransactionDto>>> HandleAsync(GetTransactionsByBankAccountCommand request, CancellationToken cancellationToken = default)
    {
        var existAccount = await _unitOfWork.BankAccountRepository.AnyAsync(x => x.Id == request.BankAccountId, cancellationToken);

        if (!existAccount)
            return Result<IEnumerable<BankTransactionDto>>.NotFound("No se ha encontrado la cuenta en el sistema");

        var entities = await _unitOfWork.BankTransactionRepository.GetByBankAccountIdAsync(request.BankAccountId, cancellationToken);

        IEnumerable<BankTransactionDto> dtos = _mapper.Map<IEnumerable<BankTransactionDto>>(entities);

        return Result.Success(dtos);
    }
}