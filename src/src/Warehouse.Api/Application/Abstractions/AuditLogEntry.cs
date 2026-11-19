using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Application.Abstractions;

/// <summary>A business audit record (who did what to which object).</summary>
public sealed record AuditLogEntry(
    TenantId TenantId,
    UserId? UserId,
    DateTimeOffset OccurredAt,
    string Action,
    string ObjectType,
    string? ObjectId,
    string? Details = null);
