using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Shared;

namespace BankApi.Infrastructure.Repository;

public class CustomerRepository(BankApiContext context) : BaseRepository<Customer>(context), IBaseRepository<Customer>;
