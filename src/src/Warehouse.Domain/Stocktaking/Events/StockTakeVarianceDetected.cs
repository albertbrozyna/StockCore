using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Stocktaking.Events;

public sealed record StockTakeVarianceDetected(TenantId TenantId, DateTimeOffset OccurredAt, StockTakeId StockTakeId, StockTakeLineId LineId, decimal Difference) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
