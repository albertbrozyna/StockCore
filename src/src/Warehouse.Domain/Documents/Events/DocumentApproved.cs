using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Documents.Events;

public sealed record DocumentApproved(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, DocumentType Type, string Number, WarehouseId WarehouseId, DateTimeOffset ApprovedAt) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
