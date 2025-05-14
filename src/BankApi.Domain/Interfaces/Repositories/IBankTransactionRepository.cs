using BankApi.Domain.Entities;

namespace BankApi.Domain.Interfaces.Repositories;

public interface IBankTransactionRepository : IBaseRepository<BankTransaction>
{
    Task<IEnumerable<BankTransaction>> GetByBankAccountIdAsync(long bankAccountId, CancellationToken cancellationToken = default);
}