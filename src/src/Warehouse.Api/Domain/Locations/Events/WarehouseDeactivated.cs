using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Locations.Events;

public sealed record WarehouseDeactivated(TenantId TenantId, DateTimeOffset OccurredAt, WarehouseId WarehouseId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
