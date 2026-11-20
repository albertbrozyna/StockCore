using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Locations.Events;

public sealed record WarehouseCreated(TenantId TenantId, DateTimeOffset OccurredAt, WarehouseId WarehouseId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
