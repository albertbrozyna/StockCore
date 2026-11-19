using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct SkuId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkuId New() => new(Guid.CreateVersion7());

    public static SkuId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
