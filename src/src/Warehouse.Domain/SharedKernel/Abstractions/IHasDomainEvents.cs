namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>An object that collects domain events until they are dispatched.</summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
