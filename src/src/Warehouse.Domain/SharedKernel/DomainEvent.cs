using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel;

/// <summary>Base record for domain events. <paramref name="OccurredAt"/> comes from the aggregate's clock.</summary>
public abstract record DomainEvent(TenantId TenantId, DateTimeOffset OccurredAt) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();
}
