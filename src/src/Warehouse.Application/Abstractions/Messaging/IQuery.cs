namespace Warehouse.Application.Abstractions.Messaging;

/// <summary>A read-only request returning <typeparamref name="TResult"/>.</summary>
#pragma warning disable CA1040 // Marker interface is the CQRS contract (ADR-004).
public interface IQuery<TResult>;
#pragma warning restore CA1040
