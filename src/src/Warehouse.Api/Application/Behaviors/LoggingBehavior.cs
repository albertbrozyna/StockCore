using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Warehouse.Api.Application.Abstractions.Messaging;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Behaviors;

/// <summary>Logs request name, duration and failure code.</summary>
public sealed partial class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> logger)
    : IPipelineBehavior<TRequest, TResult>
{
    public async Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct)
    {
        Guard.AgainstNull(next);
        var name = typeof(TRequest).Name;
        var start = Stopwatch.GetTimestamp();
        try
        {
            var result = await next().ConfigureAwait(false);
            var elapsed = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
            if (result.IsSuccess)
            {
                LogHandled(name, elapsed);
            }
            else
            {
                LogFailed(name, result.Error.Code, elapsed);
            }

            return result;
        }
        catch (Exception ex)
        {
            LogThrew(ex, name, Stopwatch.GetElapsedTime(start).TotalMilliseconds);
            throw;
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Handled {Request} in {ElapsedMs} ms")]
    private partial void LogHandled(string request, double elapsedMs);

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Request} failed with {ErrorCode} in {ElapsedMs} ms")]
    private partial void LogFailed(string request, string errorCode, double elapsedMs);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Request} threw after {ElapsedMs} ms")]
    private partial void LogThrew(Exception exception, string request, double elapsedMs);
}
