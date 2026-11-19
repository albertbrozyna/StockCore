using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record ReservationExpired(TenantId TenantId, DateTimeOffset OccurredAt, ReservationId ReservationId, string Reason) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
