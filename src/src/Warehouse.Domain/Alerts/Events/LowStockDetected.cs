using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Alerts.Events;

public sealed record LowStockDetected(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, WarehouseId WarehouseId, decimal Available, decimal Threshold) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
