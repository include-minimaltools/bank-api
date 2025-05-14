using BankApi.Domain.Interfaces.Repositories;

namespace BankApi.Domain.Interfaces;

public interface IUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; }
    public IBankAccountRepository BankAccountRepository { get; }
    public IBankTransactionRepository BankTransactionRepository { get; }

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}