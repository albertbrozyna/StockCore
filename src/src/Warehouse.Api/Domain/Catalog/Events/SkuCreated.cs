using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Catalog.Events;

public sealed record SkuCreated(TenantId TenantId, DateTimeOffset OccurredAt, SkuId SkuId, string Code) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
