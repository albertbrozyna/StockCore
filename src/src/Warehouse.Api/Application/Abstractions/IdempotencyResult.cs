namespace Warehouse.Api.Application.Abstractions;

/// <summary>Result of <see cref="IIdempotencyService.BeginAsync"/>.</summary>
public sealed record IdempotencyResult(IdempotencyStatus Status, string? StoredResponse = null);
