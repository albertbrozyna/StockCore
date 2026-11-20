using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>A fact that happened in the domain.</summary>
public interface IDomainEvent
{
    Guid EventId { get; }

    DateTimeOffset OccurredAt { get; }

    TenantId TenantId { get; }
}
