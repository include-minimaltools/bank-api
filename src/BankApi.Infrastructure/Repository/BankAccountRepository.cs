using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Interfaces.Repositories;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repository;

public class BankAccountRepository(BankApiContext context) : BaseRepository<BankAccount, BankApiContext>(context), IBankAccountRepository
{
    public async Task<IEnumerable<BankAccount>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default) 
        => await _context.BankAccounts.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
}
