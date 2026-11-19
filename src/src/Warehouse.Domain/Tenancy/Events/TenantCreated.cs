using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Tenancy.Events;

public sealed record TenantCreated(TenantId TenantId, DateTimeOffset OccurredAt) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
