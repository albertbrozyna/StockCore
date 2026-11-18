using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence;

/// <summary>Default dispatcher that drops events until in-process event handlers exist.</summary>
public sealed class NoOpDomainEventDispatcher : IDomainEventDispatcher
{
    public Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct) => Task.CompletedTask;
}
