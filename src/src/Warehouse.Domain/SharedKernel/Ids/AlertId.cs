using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct AlertId(Guid Value) : IStronglyTypedId<Guid>
{
    public static AlertId New() => new(Guid.CreateVersion7());

    public static AlertId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
