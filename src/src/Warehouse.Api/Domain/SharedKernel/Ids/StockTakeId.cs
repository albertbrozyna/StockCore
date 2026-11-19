using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct StockTakeId(Guid Value) : IStronglyTypedId<Guid>
{
    public static StockTakeId New() => new(Guid.CreateVersion7());

    public static StockTakeId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
