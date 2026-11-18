using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken ct);
}
