using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct StockTakeLineId(Guid Value) : IStronglyTypedId<Guid>
{
    public static StockTakeLineId New() => new(Guid.CreateVersion7());

    public static StockTakeLineId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
