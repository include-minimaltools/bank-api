using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces;

public interface IUnitOfWork
{
    public IBaseRepository<Customer> CustomerRepository { get; }
    public IBaseRepository<BankAccount> BankAccountRepository { get; }
    public IBaseRepository<BankTransaction> BankTransactionRepository { get; }

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}