using System.Linq.Expressions;

namespace BankApi.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
}