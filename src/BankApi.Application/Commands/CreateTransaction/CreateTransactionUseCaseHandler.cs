using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Commands.CreateTransaction;

public class CreateTransactionUseCaseHandler(IUnitOfWork unitOfWork, IIdGenerator idGenerator) : IUseCaseHandler<CreateTransactionUseCaseCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdGenerator _idGenerator = idGenerator;

    public async Task<Result<long>> HandleAsync(CreateTransactionUseCaseCommand request, CancellationToken cancellationToken = default)
    {
        BankAccount? account = await _unitOfWork.BankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (account is null)
            return Result<long>.NotFound("La cuenta a la que desea depositar no existe");

        if (request.Amount < 0)
            return Result<long>.ValidationError(new ValidationError("amount", "El monto no puede ser negativo"));

        BankTransaction transaction = new()
        {
            Id = _idGenerator.NewId(),
            TransactionType = request.TransactionType,
            Amount = request.Amount,
        };

        if (request.TransactionType == "D")
            account.Balance += request.Amount;

        else
        {
            if (request.Amount > account.Balance)
                return Result<long>.ValidationError(new ValidationError("amount", "El saldo es insuficiente para realizar el retiro"));

            account.Balance -= request.Amount;
        }

        transaction.Balance = account.Balance;
        account.BankTransactions.Add(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}