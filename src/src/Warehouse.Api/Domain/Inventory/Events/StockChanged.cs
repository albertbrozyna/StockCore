using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record StockChanged(TenantId TenantId, DateTimeOffset OccurredAt, StockLevelId StockLevelId, decimal OnHand, decimal Reserved) : DomainEvent(TenantId, OccurredAt);
