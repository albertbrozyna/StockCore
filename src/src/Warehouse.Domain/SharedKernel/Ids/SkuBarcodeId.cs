using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct SkuBarcodeId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkuBarcodeId New() => new(Guid.CreateVersion7());

    public static SkuBarcodeId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
