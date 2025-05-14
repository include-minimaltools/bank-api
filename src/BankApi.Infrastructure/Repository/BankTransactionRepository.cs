using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Shared;

namespace BankApi.Infrastructure.Repository;

public class BankTransactionRepository(BankApiContext context) : BaseRepository<BankTransaction>(context), IBaseRepository<BankTransaction>;
