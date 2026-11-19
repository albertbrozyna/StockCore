using Microsoft.Extensions.DependencyInjection;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Abstractions.Messaging;

internal sealed class CommandInvoker<TCommand, TResult> : RequestInvoker<TResult>
    where TCommand : ICommand<TResult>
{
    public override Task<Result<TResult>> InvokeAsync(object request, IServiceProvider services, CancellationToken ct)
    {
        var handler = services.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        var command = (TCommand)request;
        return RunPipelineAsync(services, command, () => handler.HandleAsync(command, ct), ct);
    }
}
