using Warehouse.Application.Abstractions;
using Warehouse.Application.Abstractions.Messaging;
using Warehouse.Domain.SharedKernel;

namespace Warehouse.Application.Behaviors;

/// <summary>Rejects requests that run without a resolved tenant.</summary>
public sealed class TenantGuardBehavior<TRequest, TResult>(ITenantContext tenantContext)
    : IPipelineBehavior<TRequest, TResult>
{
    public Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct)
    {
        Guard.AgainstNull(next);
        return tenantContext.IsResolved
            ? next()
            : Task.FromResult(Result.Failure<TResult>(new Error("Tenant.NotResolved", "The tenant of the request could not be resolved.", ErrorType.Forbidden)));
    }
}
