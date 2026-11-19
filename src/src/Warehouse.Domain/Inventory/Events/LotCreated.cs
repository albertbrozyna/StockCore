using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

public sealed record LotCreated(TenantId TenantId, DateTimeOffset OccurredAt, LotId LotId, SkuId SkuId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
