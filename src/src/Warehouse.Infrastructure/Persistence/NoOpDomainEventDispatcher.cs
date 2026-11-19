using Warehouse.Application.Abstractions;
using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Infrastructure.Persistence;

/// <summary>Default dispatcher that drops events until in-process event handlers exist.</summary>
public sealed class NoOpDomainEventDispatcher : IDomainEventDispatcher
{
    public Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct) => Task.CompletedTask;
}
