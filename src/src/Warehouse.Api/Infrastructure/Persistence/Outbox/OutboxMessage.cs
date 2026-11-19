using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Infrastructure.Persistence.Outbox;

/// <summary>An integration event waiting to be published. Written in the same transaction as the change that raised it.</summary>
public sealed class OutboxMessage
{
    private OutboxMessage()
    {
        Type = string.Empty;
        Payload = string.Empty;
    }

    public Guid Id { get; private set; }

    public TenantId TenantId { get; private set; }

    /// <summary>Full CLR name of the event type.</summary>
    public string Type { get; private set; }

    /// <summary>JSON payload (jsonb).</summary>
    public string Payload { get; private set; }

    public DateTimeOffset OccurredAt { get; private set; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public int Attempts { get; private set; }

    public string? LastError { get; private set; }

    public static OutboxMessage Create(IIntegrationEvent integrationEvent)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);
        return new OutboxMessage
        {
            Id = integrationEvent.EventId,
            TenantId = integrationEvent.TenantId,
            Type = integrationEvent.GetType().FullName!,
            Payload = OutboxJson.Serialize(integrationEvent),
            OccurredAt = integrationEvent.OccurredAt,
        };
    }

    public void MarkProcessed(DateTimeOffset at)
    {
        ProcessedAt = at;
        Attempts++;
        LastError = null;
    }

    public void MarkFailed(string error)
    {
        Attempts++;
        LastError = Guard.AgainstNullOrWhiteSpace(error);
    }
}
