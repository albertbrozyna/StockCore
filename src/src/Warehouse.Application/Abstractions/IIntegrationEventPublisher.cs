namespace Warehouse.Application.Abstractions;

/// <summary>Publishes integration events by writing them to the outbox in the current transaction.</summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken ct)
        where T : notnull;
}
