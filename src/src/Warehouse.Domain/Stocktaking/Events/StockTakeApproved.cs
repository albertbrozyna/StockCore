using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Stocktaking.Events;

public sealed record StockTakeApproved(TenantId TenantId, DateTimeOffset OccurredAt, StockTakeId StockTakeId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
