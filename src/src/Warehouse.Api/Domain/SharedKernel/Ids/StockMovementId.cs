using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct StockMovementId(Guid Value) : IStronglyTypedId<Guid>
{
    public static StockMovementId New() => new(Guid.CreateVersion7());

    public static StockMovementId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
