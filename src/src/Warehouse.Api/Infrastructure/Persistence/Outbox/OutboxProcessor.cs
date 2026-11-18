using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Warehouse.Api.Application.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Outbox;

/// <summary>
/// Polls the outbox and hands pending messages to <see cref="IOutboxMessageDispatcher"/>.
/// Skeleton: assumes a single running instance (no row locking yet).
/// </summary>
public sealed partial class OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger) : BackgroundService
{
    private const int BatchSize = 20;
    private const int MaxAttempts = 10;
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(PollInterval);
        try
        {
            do
            {
                try
                {
                    await ProcessBatchAsync(stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    LogBatchFailed(ex);
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false));
        }
        catch (OperationCanceledException)
        {
            // Host is stopping.
        }
    }

    /// <summary>Processes one batch and returns the number of messages handled.</summary>
    internal async Task<int> ProcessBatchAsync(CancellationToken ct)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var db = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IOutboxMessageDispatcher>();
            var clock = scope.ServiceProvider.GetRequiredService<IClock>();

            var messages = await db.OutboxMessages
                .Where(m => m.ProcessedAt == null && m.Attempts < MaxAttempts)
                .OrderBy(m => m.OccurredAt)
                .Take(BatchSize)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            foreach (var message in messages)
            {
                try
                {
                    await dispatcher.DispatchAsync(message, ct).ConfigureAwait(false);
                    message.MarkProcessed(clock.UtcNow);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    message.MarkFailed(ex.Message);
                    LogMessageFailed(ex, message.Id);
                }
            }

            if (messages.Count > 0)
            {
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
            }

            return messages.Count;
        }
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Outbox batch failed")]
    private partial void LogBatchFailed(Exception exception);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Outbox message {MessageId} failed to dispatch")]
    private partial void LogMessageFailed(Exception exception, Guid messageId);
}
