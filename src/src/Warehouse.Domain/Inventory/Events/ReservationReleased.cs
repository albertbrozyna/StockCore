using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record ReservationReleased(TenantId TenantId, DateTimeOffset OccurredAt, ReservationId ReservationId, string Reason) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
