using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Domain.SharedKernel;

namespace Warehouse.Infrastructure.Persistence.Interceptors;

/// <summary>Rejects updates and deletes of append-only (<c>ImmutableTenantEntity</c>) entries.</summary>
public sealed class ImmutableEntityGuard : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Check(eventData?.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Check(eventData?.Context);
        return ValueTask.FromResult(result);
    }

    internal static void Check(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var violation = context.ChangeTracker.Entries()
            .FirstOrDefault(e => e.State is EntityState.Modified or EntityState.Deleted && e.Entity is IImmutableEntity);
        if (violation is not null)
        {
            throw new DomainException(new Error(
                "Persistence.ImmutableEntity",
                $"{violation.Entity.GetType().Name} is append-only and cannot be {violation.State.ToString().ToLowerInvariant()}.",
                ErrorType.Conflict));
        }
    }
}
