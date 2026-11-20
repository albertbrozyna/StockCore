namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>Marker for strongly-typed identifiers wrapping a primitive value.</summary>
public interface IStronglyTypedId<TValue>
    where TValue : notnull
{
    TValue Value { get; }
}
