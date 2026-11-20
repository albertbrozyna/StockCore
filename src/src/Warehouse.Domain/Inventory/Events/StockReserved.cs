using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record StockReserved(TenantId TenantId, DateTimeOffset OccurredAt, ReservationId ReservationId, IReadOnlyList<StockReservedLine> Lines) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
