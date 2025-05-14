using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Interfaces.Repositories;
using BankApi.Infrastructure.Contexts;

public class UnitOfWork(ICustomerRepository customerRepository, IBankAccountRepository bankAccountRepository, IBankTransactionRepository bankTransactionRepository, BankApiContext context) : IUnitOfWork
{
    public ICustomerRepository CustomerRepository => customerRepository;

    public IBankAccountRepository BankAccountRepository => bankAccountRepository;

    public IBankTransactionRepository BankTransactionRepository => bankTransactionRepository;

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}