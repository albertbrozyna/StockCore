using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct SkuUnitConversionId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkuUnitConversionId New() => new(Guid.CreateVersion7());

    public static SkuUnitConversionId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
