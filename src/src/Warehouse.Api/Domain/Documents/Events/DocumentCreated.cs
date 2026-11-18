using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Documents.Events;

public sealed record DocumentCreated(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, DocumentType Type) : DomainEvent(TenantId, OccurredAt);
