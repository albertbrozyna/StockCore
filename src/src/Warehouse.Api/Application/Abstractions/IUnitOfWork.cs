namespace Warehouse.Api.Application.Abstractions;

/// <summary>Persistence unit of work with explicit transaction control.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);

    Task BeginTransactionAsync(CancellationToken ct);

    Task CommitAsync(CancellationToken ct);

    Task RollbackAsync(CancellationToken ct);
}
