using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Interfaces.Repositories;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repository;

public class CustomerRepository(BankApiContext context) : BaseRepository<Customer, BankApiContext>(context), ICustomerRepository
{
    public override Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => _context.Customers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    
}
