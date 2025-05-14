
using BankApi.Domain.Shared;

namespace BankApi.Application.Interfaces;

public interface IBaseService<TDto, TId> : IBaseService<TDto, TId, TDto>
    where TDto : class;

public interface IBaseService<TDto, TId, TRequest>
    where TRequest : class
    where TDto : class
{

    Task<Result<IEnumerable<TDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<TDto>> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<Result<TId>> CreateAsync(TRequest dto, CancellationToken cancellationToken = default);

    Task<Result<Unit>> UpdateAsync(TId id, TRequest dto, CancellationToken cancellationToken = default);

    Task<Result<Unit>> HardDeleteAsync(TId id, CancellationToken cancellationToken = default);
}
