using FluentValidation;
using Warehouse.Api.Application.Abstractions.Messaging;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Behaviors;

/// <summary>Runs all FluentValidation validators for the request; failures become a <c>Validation.Failed</c> result.</summary>
public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
{
    public async Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct)
    {
        Guard.AgainstNull(next);
        var context = new ValidationContext<TRequest>(request);
        var failures = new List<string>();
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context, ct).ConfigureAwait(false);
            failures.AddRange(result.Errors.Select(e => e.ErrorMessage));
        }

        return failures.Count == 0
            ? await next().ConfigureAwait(false)
            : Result.Failure<TResult>(new Error("Validation.Failed", string.Join("; ", failures), ErrorType.Validation));
    }
}
