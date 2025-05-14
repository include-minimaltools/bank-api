using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Application.Commands.CreateTransaction;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Commands.ApplyInterest;

public class ApplyInterestUseCaseHandler(IUnitOfWork unitOfWork, IIdGenerator idGenerator) : IUseCaseHandler<ApplyInterestUseCaseCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdGenerator _idGenerator = idGenerator;

    public async Task<Result<long>> HandleAsync(ApplyInterestUseCaseCommand request, CancellationToken cancellationToken = default)
    {
        BankAccount? account = await _unitOfWork.BankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (account is null)
            return Result<long>.NotFound("La cuenta a la que desea depositar no existe");

        var now = DateTime.UtcNow;

        bool alreadyApplied = account.BankTransactions.Any(t =>
            t.TransactionType == "I" &&
            (
                (account.InterestType == "M" &&
                 t.CreatedAt.Year == now.Year &&
                 t.CreatedAt.Month == now.Month)
                ||
                (account.InterestType == "Y" &&
                 t.CreatedAt.Year == now.Year)
            ));

        var interestAmount = account.Balance * account.InteresRate;

        account.Balance += interestAmount;

        BankTransaction transaction = new()
        {
            Id = _idGenerator.NewId(),
            TransactionType = "I",
            Amount = interestAmount,
            Balance = account.Balance,
            CreatedAt = DateTime.UtcNow
        };

        account.BankTransactions.Add(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}