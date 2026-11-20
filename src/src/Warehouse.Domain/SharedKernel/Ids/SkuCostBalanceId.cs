using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct SkuCostBalanceId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkuCostBalanceId New() => new(Guid.CreateVersion7());

    public static SkuCostBalanceId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
