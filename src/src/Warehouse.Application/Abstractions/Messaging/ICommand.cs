namespace Warehouse.Application.Abstractions.Messaging;

/// <summary>A request that changes state and returns <typeparamref name="TResult"/>.</summary>
#pragma warning disable CA1040 // Marker interface is the CQRS contract (ADR-004).
public interface ICommand<TResult>;
#pragma warning restore CA1040
