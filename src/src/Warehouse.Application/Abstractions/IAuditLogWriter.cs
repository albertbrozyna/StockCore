namespace Warehouse.Application.Abstractions;

/// <summary>Persists business audit records.</summary>
public interface IAuditLogWriter
{
    Task WriteAsync(AuditLogEntry entry, CancellationToken ct);
}
