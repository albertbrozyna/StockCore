using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Documents.Events;

public sealed record DocumentCancelled(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, string Reason) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
