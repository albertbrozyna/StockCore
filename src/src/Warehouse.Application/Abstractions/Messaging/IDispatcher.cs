using Warehouse.Domain.SharedKernel;

namespace Warehouse.Application.Abstractions.Messaging;

/// <summary>Sends commands and queries through the behavior pipeline to their handlers.</summary>
public interface IDispatcher
{
    Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct);

    Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct);
}
