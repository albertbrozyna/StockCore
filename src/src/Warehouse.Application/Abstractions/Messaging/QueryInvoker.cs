using Microsoft.Extensions.DependencyInjection;
using Warehouse.Domain.SharedKernel;

namespace Warehouse.Application.Abstractions.Messaging;

internal sealed class QueryInvoker<TQuery, TResult> : RequestInvoker<TResult>
    where TQuery : IQuery<TResult>
{
    public override Task<Result<TResult>> InvokeAsync(object request, IServiceProvider services, CancellationToken ct)
    {
        var handler = services.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        var query = (TQuery)request;
        return RunPipelineAsync(services, query, () => handler.HandleAsync(query, ct), ct);
    }
}
