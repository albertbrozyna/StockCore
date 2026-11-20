using Warehouse.Application.Abstractions;
using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Infrastructure.Persistence.Outbox;

/// <summary>Writes integration events to the outbox; they are saved with the surrounding unit of work.</summary>
public sealed class OutboxIntegrationEventPublisher(WarehouseDbContext dbContext) : IIntegrationEventPublisher
{
    public Task PublishAsync<T>(T @event, CancellationToken ct)
        where T : notnull
    {
        if (@event is not IIntegrationEvent integrationEvent)
        {
            throw new ArgumentException($"{typeof(T).Name} is not an integration event.", nameof(@event));
        }

        dbContext.OutboxMessages.Add(OutboxMessage.Create(integrationEvent));
        return Task.CompletedTask;
    }
}
