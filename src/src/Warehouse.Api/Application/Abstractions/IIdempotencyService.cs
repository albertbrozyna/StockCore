namespace Warehouse.Api.Application.Abstractions;

/// <summary>Guards commands against duplicate execution by an idempotency key.</summary>
public interface IIdempotencyService
{
    Task<IdempotencyResult> BeginAsync(string key, string requestHash, CancellationToken ct);

    Task CompleteAsync(string key, string response, CancellationToken ct);
}
