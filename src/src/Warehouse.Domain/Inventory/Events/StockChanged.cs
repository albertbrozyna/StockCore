using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record StockChanged(TenantId TenantId, DateTimeOffset OccurredAt, StockLevelId StockLevelId, decimal OnHand, decimal Reserved) : DomainEvent(TenantId, OccurredAt);
