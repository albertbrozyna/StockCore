namespace Warehouse.Application.Abstractions;

/// <summary>State of an idempotency key when a request begins.</summary>
public enum IdempotencyStatus
{
    /// <summary>The key is new; the request should run.</summary>
    Started,

    /// <summary>The same request already completed; <see cref="IdempotencyResult.StoredResponse"/> holds its response.</summary>
    Completed,

    /// <summary>The same request is still being processed.</summary>
    InProgress,

    /// <summary>The key was used with a different request payload.</summary>
    HashMismatch,
}
