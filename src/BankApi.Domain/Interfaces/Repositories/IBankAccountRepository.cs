using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces.Repositories;

public interface IBankAccountRepository : IBaseRepository<BankAccount>
{
    Task<IEnumerable<BankAccount>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default);
}