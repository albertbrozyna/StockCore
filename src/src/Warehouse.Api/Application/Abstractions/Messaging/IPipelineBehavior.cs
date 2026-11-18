using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Abstractions.Messaging;

/// <summary>Cross-cutting step wrapped around a handler. Call <paramref name="next"/> to continue the pipeline.</summary>
public interface IPipelineBehavior<in TRequest, TResult>
{
    Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct);
}
