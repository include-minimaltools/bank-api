using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces.Repositories;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repository;

public class BankTransactionRepository(BankApiContext context) : BaseRepository<BankTransaction, BankApiContext>(context), IBankTransactionRepository
{
    public async Task<IEnumerable<BankTransaction>> GetByBankAccountIdAsync(long bankAccountId, CancellationToken cancellationToken = default)
        => await _context.BankTransactions.Where(x => x.BankAccountId == bankAccountId).ToListAsync(cancellationToken);
}
