using BankApi.Domain.Shared;

namespace BankApi.Domain.Interfaces;

public interface IUseCaseHandler<TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IUseCaseHandler<TRequest> : IUseCaseHandler<TRequest, Unit>;