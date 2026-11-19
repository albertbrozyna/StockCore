using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct LotId(Guid Value) : IStronglyTypedId<Guid>
{
    public static LotId New() => new(Guid.CreateVersion7());

    public static LotId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
