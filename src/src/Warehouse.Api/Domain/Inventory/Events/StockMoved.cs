using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record StockMoved(TenantId TenantId, DateTimeOffset OccurredAt, StockMovementId MovementId, SkuId SkuId, LocationId LocationId, decimal QuantityDelta, MovementType Type) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
