using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Application.Abstractions;

/// <summary>Dispatches domain events collected after <c>SaveChanges</c> to in-process handlers.</summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct);
}
