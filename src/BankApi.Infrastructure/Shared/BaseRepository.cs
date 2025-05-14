using System.Linq.Expressions;
using BankApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Shared;

public abstract class BaseRepository<T>(DbContext context) : IBaseRepository<T> where T : class
{
    protected readonly DbContext _context = context;
    protected readonly DbSet<T> _set = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _set.FindAsync([id], cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _set.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _set.ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        _set.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        _set.Remove(entity);
    }
}
