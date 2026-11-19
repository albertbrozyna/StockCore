using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Fulfillment.Events;

public sealed record PutawayCompleted(TenantId TenantId, DateTimeOffset OccurredAt, PutawayTaskId PutawayTaskId, LocationId ActualLocationId) : DomainEvent(TenantId, OccurredAt);
