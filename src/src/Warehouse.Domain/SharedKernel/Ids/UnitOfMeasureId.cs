using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct UnitOfMeasureId(Guid Value) : IStronglyTypedId<Guid>
{
    public static UnitOfMeasureId New() => new(Guid.CreateVersion7());

    public static UnitOfMeasureId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
