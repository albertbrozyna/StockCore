using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Tenancy.Events;

public sealed record TenantSuspended(TenantId TenantId, DateTimeOffset OccurredAt) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
