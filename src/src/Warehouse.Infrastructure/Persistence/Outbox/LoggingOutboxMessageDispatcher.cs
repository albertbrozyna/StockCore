using Microsoft.Extensions.Logging;

namespace Warehouse.Infrastructure.Persistence.Outbox;

/// <summary>Placeholder transport that only logs; replaced when the Service Bus integration is added.</summary>
public sealed partial class LoggingOutboxMessageDispatcher(ILogger<LoggingOutboxMessageDispatcher> logger) : IOutboxMessageDispatcher
{
    public Task DispatchAsync(OutboxMessage message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        LogDispatched(message.Type, message.Id);
        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Outbox message {Type} ({MessageId}) dispatched")]
    private partial void LogDispatched(string type, Guid messageId);
}
