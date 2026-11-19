using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct WarehouseId(Guid Value) : IStronglyTypedId<Guid>
{
    public static WarehouseId New() => new(Guid.CreateVersion7());

    public static WarehouseId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
