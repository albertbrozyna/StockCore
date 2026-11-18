using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Documents.Events;

public sealed record DocumentApproved(TenantId TenantId, DateTimeOffset OccurredAt, DocumentId DocumentId, DocumentType Type, string Number, WarehouseId WarehouseId, DateTimeOffset ApprovedAt) : DomainEvent(TenantId, OccurredAt), IIntegrationEvent;
