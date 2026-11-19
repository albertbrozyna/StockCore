using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record NegativeStockDetected(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, LocationId LocationId, decimal OnHand) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
