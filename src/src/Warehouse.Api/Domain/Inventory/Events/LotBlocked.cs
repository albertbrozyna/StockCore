using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Inventory.Events;

public sealed record LotBlocked(TenantId TenantId, DateTimeOffset OccurredAt, LotId LotId, SkuId SkuId) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
