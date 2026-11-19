using Microsoft.EntityFrameworkCore.Storage;
using Warehouse.Api.Application.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence;

/// <summary><see cref="IUnitOfWork"/> over <see cref="WarehouseDbContext"/> with one explicit transaction at a time.</summary>
public sealed class EfUnitOfWork(WarehouseDbContext dbContext) : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _transaction;

    public Task<int> SaveChangesAsync(CancellationToken ct) => dbContext.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("A transaction is already active.");
        }

        _transaction = await dbContext.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("No active transaction.");
        }

        try
        {
            await _transaction.CommitAsync(ct).ConfigureAwait(false);
        }
        finally
        {
            await DisposeTransactionAsync().ConfigureAwait(false);
        }
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (_transaction is null)
        {
            return;
        }

        try
        {
            await _transaction.RollbackAsync(ct).ConfigureAwait(false);
        }
        finally
        {
            await DisposeTransactionAsync().ConfigureAwait(false);
        }
    }

    /// <summary>Releases a transaction left open (e.g. after an unhandled exception); the database rolls it back.</summary>
    public ValueTask DisposeAsync() => new(DisposeTransactionAsync());

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }
}
