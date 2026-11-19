using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Abstractions.Messaging;

/// <summary>Closed-generic bridge between the untyped dispatcher call and a typed handler + pipeline.</summary>
internal abstract class RequestInvoker<TResult>
{
    public abstract Task<Result<TResult>> InvokeAsync(object request, IServiceProvider services, CancellationToken ct);

    /// <summary>Wraps the handler in the registered behaviors; the first registered behavior is the outermost.</summary>
    protected static Task<Result<TResult>> RunPipelineAsync<TRequest>(
        IServiceProvider services,
        TRequest request,
        Func<Task<Result<TResult>>> handler,
        CancellationToken ct)
        where TRequest : notnull
    {
        var behaviors = (IPipelineBehavior<TRequest, TResult>[])
            [.. (IEnumerable<IPipelineBehavior<TRequest, TResult>>?)services.GetService(typeof(IEnumerable<IPipelineBehavior<TRequest, TResult>>)) ?? []];

        var next = handler;
        for (var i = behaviors.Length - 1; i >= 0; i--)
        {
            var behavior = behaviors[i];
            var inner = next;
            next = () => behavior.HandleAsync(request, inner, ct);
        }

        return next();
    }
}
