using System.Collections.Concurrent;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Abstractions.Messaging;

/// <summary>Resolves handlers from DI and runs them through the registered pipeline behaviors.</summary>
public sealed class Dispatcher(IServiceProvider services) : IDispatcher
{
    private static readonly ConcurrentDictionary<(Type Request, Type Result, bool IsCommand), object> Invokers = new();

    public Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct)
    {
        Guard.AgainstNull(command);
        return GetInvoker<TResult>(command.GetType(), isCommand: true).InvokeAsync(command, services, ct);
    }

    public Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
    {
        Guard.AgainstNull(query);
        return GetInvoker<TResult>(query.GetType(), isCommand: false).InvokeAsync(query, services, ct);
    }

    private static RequestInvoker<TResult> GetInvoker<TResult>(Type requestType, bool isCommand) =>
        (RequestInvoker<TResult>)Invokers.GetOrAdd(
            (requestType, typeof(TResult), isCommand),
            static key => Activator.CreateInstance(
                (key.IsCommand ? typeof(CommandInvoker<,>) : typeof(QueryInvoker<,>)).MakeGenericType(key.Request, key.Result))!);
}
