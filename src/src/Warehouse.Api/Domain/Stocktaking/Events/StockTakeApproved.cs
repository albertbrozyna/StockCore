using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Stocktaking.Events;

public sealed record StockTakeApproved(TenantId TenantId, DateTimeOffset OccurredAt, StockTakeId StockTakeId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
