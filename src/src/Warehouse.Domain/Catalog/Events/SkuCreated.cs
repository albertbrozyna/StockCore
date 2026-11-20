using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Catalog.Events;

public sealed record SkuCreated(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, string Code) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
