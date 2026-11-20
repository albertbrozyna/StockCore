using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Infrastructure.Persistence.Outbox;

namespace Warehouse.Infrastructure.Persistence.Interceptors;

/// <summary>Copies integration events raised by tracked entities into <see cref="OutboxMessage"/> rows saved in the same transaction.</summary>
/// <remarks>Must run before <see cref="DomainEventsInterceptor"/>, which clears the events.</remarks>
public sealed class OutboxInterceptor : SaveChangesInterceptor
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

    internal static void Apply(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var events = context.ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvents>()
            .SelectMany(e => e.DomainEvents)
            .OfType<IIntegrationEvent>()
            .ToList();

        foreach (var integrationEvent in events)
        {
            context.Set<OutboxMessage>().Add(OutboxMessage.Create(integrationEvent));
        }
    }
}
