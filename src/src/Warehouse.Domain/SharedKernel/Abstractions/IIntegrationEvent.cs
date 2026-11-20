namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>Marker for domain events that are also published to the outside through the outbox.</summary>
public interface IIntegrationEvent : IDomainEvent;
