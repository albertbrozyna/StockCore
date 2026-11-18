using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Interceptors;

/// <summary>Blocks writes of tenant-owned entities that belong to a different tenant than the current context (or when none is resolved).</summary>
public sealed class TenantSaveChangesGuard(ITenantContext tenantContext) : SaveChangesInterceptor
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

    internal void Check(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            if (entry.Entity is not IHasTenant owned)
            {
                continue;
            }

            if (!tenantContext.IsResolved || owned.TenantId != tenantContext.TenantId)
            {
                throw new DomainException(new Error(
                    "Tenancy.CrossTenantWrite",
                    $"{entry.Entity.GetType().Name} does not belong to the current tenant.",
                    ErrorType.Forbidden));
            }
        }
    }
}
