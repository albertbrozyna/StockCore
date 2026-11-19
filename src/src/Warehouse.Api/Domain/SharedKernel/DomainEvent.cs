using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Base record for domain events. <paramref name="OccurredAt"/> comes from the aggregate's clock.</summary>
public abstract record DomainEvent(TenantId TenantId, DateTimeOffset OccurredAt) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();
}
