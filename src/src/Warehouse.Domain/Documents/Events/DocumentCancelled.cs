using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Documents.Events;

public sealed record DocumentCancelled(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, string Reason) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
