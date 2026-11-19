using Warehouse.Application.Abstractions;
using Warehouse.Application.Abstractions.Messaging;
using Warehouse.Domain.SharedKernel;

namespace Warehouse.Application.Behaviors;

/// <summary>Wraps a command in one transaction: saves and commits on success, rolls back on failure or exception.</summary>
public sealed class TransactionBehavior<TRequest, TResult>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : ICommand<TResult>
{
    public async Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct)
    {
        Guard.AgainstNull(next);
        await unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);
        try
        {
            var result = await next().ConfigureAwait(false);
            if (result.IsFailure)
            {
                await unitOfWork.RollbackAsync(ct).ConfigureAwait(false);
                return result;
            }

            await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
            await unitOfWork.CommitAsync(ct).ConfigureAwait(false);
            return result;
        }
        catch
        {
            await unitOfWork.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
            throw;
        }
    }
}
