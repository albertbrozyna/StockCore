using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record StockMoved(TenantId TenantId, DateTimeOffset OccurredAt, StockMovementId MovementId, SkuId SkuId, LocationId LocationId, decimal QuantityDelta, MovementType Type) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
