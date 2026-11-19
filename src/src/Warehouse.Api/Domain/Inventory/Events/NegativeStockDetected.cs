using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record NegativeStockDetected(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, LocationId LocationId, decimal OnHand) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
