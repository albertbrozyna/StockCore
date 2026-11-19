namespace Warehouse.Api.Infrastructure.Persistence.Outbox;

/// <summary>Sends a stored outbox message to the external transport (Service Bus). Throw to retry later.</summary>
public interface IOutboxMessageDispatcher
{
    Task DispatchAsync(OutboxMessage message, CancellationToken ct);
}
