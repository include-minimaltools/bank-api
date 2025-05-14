using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Commands.CreateDeposit;

public class CreateDepositUseCaseHandler(IUnitOfWork unitOfWork, IIdGenerator idGenerator) : IUseCaseHandler<CreateDepositUseCaseCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdGenerator _idGenerator = idGenerator;

    public async Task<Result<long>> HandleAsync(CreateDepositUseCaseCommand request, CancellationToken cancellationToken = default)
    {
        BankAccount? account = await _unitOfWork.BankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (account is null)
            return Result<long>.NotFound("La cuenta a la que desea depositar no existe");

        BankTransaction transaction = new()
        {
            Id = _idGenerator.NewId(),
            TransactionType = "D",
            Amount = request.Amount,
        };

        account.Balance += request.Amount;
        account.BankTransactions.Add(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}