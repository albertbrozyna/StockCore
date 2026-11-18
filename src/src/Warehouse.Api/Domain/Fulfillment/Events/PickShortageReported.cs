using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Fulfillment.Events;

public sealed record PickShortageReported(TenantId TenantId, DateTimeOffset OccurredAt, PickingOrderId PickingOrderId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
