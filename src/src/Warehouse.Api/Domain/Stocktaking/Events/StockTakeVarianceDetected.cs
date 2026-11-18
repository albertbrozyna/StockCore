using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Stocktaking.Events;

public sealed record StockTakeVarianceDetected(TenantId TenantId, DateTimeOffset OccurredAt, StockTakeId StockTakeId, StockTakeLineId LineId, decimal Difference) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
