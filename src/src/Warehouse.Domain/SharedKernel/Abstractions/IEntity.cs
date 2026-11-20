namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>An entity identified by <typeparamref name="TId"/>.</summary>
public interface IEntity<out TId>
    where TId : notnull
{
    TId Id { get; }
}
