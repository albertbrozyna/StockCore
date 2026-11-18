using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Interceptors;

/// <summary>Stamps <c>Created*</c> / <c>Modified*</c> audit data and assigns <c>TenantId</c> from the tenant context to new entities.</summary>
public sealed class AuditingInterceptor(IClock clock, ICurrentUser currentUser, ITenantContext tenantContext) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Apply(eventData?.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData?.Context);
        return ValueTask.FromResult(result);
    }

    internal void Apply(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var now = clock.UtcNow;
        var user = currentUser.UserId;
        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreationStamp created)
                    {
                        created.StampCreated(now, user);
                    }

                    if (entry.Entity is ITenantStamp tenant
                        && entry.Entity is IHasTenant { TenantId.IsEmpty: true }
                        && tenantContext.IsResolved)
                    {
                        tenant.SetTenant(tenantContext.TenantId);
                    }

                    break;
                case EntityState.Modified when entry.Entity is IModificationStamp modified:
                    modified.StampModified(now, user);
                    break;
                default:
                    break;
            }
        }
    }
}
