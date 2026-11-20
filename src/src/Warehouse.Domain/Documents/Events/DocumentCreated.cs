using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Documents.Events;

public sealed record DocumentCreated(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, DocumentType Type) : DomainEvent(TenantId, OccurredAt);
