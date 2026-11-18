using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Interceptors;

/// <summary>Collects domain events before saving and dispatches them to in-process handlers after a successful save (same transaction).</summary>
public sealed class DomainEventsInterceptor(IDomainEventDispatcher dispatcher) : SaveChangesInterceptor
{
    private List<IDomainEvent> _pending = [];

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Collect(eventData?.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Collect(eventData?.Context);
        return ValueTask.FromResult(result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var events = TakePending();
        if (events.Count > 0)
        {
            dispatcher.DispatchAsync(events, CancellationToken.None).GetAwaiter().GetResult();
        }

        return result;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var events = TakePending();
        if (events.Count > 0)
        {
            await dispatcher.DispatchAsync(events, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData) => _pending = [];

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _pending = [];
        return Task.CompletedTask;
    }

    internal void Collect(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var holder in context.ChangeTracker.Entries().Select(e => e.Entity).OfType<IHasDomainEvents>().ToList())
        {
            _pending.AddRange(holder.DomainEvents);
            holder.ClearDomainEvents();
        }
    }

    private List<IDomainEvent> TakePending()
    {
        var events = _pending;
        _pending = [];
        return events;
    }
}
