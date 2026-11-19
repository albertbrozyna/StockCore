using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Fulfillment.Events;

public sealed record PutawayCompleted(TenantId TenantId, DateTimeOffset OccurredAt, PutawayTaskId PutawayTaskId, LocationId ActualLocationId) : DomainEvent(TenantId, OccurredAt);
