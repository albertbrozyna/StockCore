using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record StockReserved(TenantId TenantId, DateTimeOffset OccurredAt, ReservationId ReservationId, IReadOnlyList<StockReservedLine> Lines) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
