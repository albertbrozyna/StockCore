using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct SerialItemId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SerialItemId New() => new(Guid.CreateVersion7());

    public static SerialItemId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
