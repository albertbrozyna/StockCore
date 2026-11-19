using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Alerts.Events;

public sealed record LowStockDetected(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, WarehouseId WarehouseId, decimal Available, decimal Threshold) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
