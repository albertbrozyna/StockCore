using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Fulfillment.Events;

public sealed record PickShortageReported(TenantId TenantId, DateTimeOffset OccurredAt, PickingOrderId PickingOrderId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
