using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Application.Abstractions;
using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Infrastructure.Persistence.Interceptors;

/// <summary>Turns physical deletes of soft-deletable entities into updates and stamps <c>DeletedAt/DeletedBy</c>.</summary>
public sealed class SoftDeleteInterceptor(IClock clock, ICurrentUser currentUser) : SaveChangesInterceptor
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
        foreach (var entry in context.ChangeTracker.Entries().Where(e => e.Entity is IDeletionStamp).ToList())
        {
            var stamp = (IDeletionStamp)entry.Entity;
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                stamp.Delete();
                stamp.StampDeleted(now, user);
            }
            else if (entry.State == EntityState.Modified && entry.Entity is ISoftDeletable { IsDeleted: true, DeletedAt: null })
            {
                stamp.StampDeleted(now, user);
            }
        }
    }
}
