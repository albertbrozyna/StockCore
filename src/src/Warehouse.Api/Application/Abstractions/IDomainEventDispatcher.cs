using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Application.Abstractions;

/// <summary>Dispatches domain events collected after <c>SaveChanges</c> to in-process handlers.</summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct);
}
