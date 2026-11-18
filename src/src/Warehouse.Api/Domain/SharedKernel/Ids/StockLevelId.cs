using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct StockLevelId(Guid Value) : IStronglyTypedId<Guid>
{
    public static StockLevelId New() => new(Guid.CreateVersion7());

    public static StockLevelId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
