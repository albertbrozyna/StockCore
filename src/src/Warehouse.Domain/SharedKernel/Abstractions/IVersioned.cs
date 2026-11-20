namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>Optimistic concurrency token (mapped to PostgreSQL <c>xmin</c>).</summary>
public interface IVersioned
{
    uint Version { get; }
}
