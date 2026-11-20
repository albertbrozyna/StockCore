using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct PutawayTaskId(Guid Value) : IStronglyTypedId<Guid>
{
    public static PutawayTaskId New() => new(Guid.CreateVersion7());

    public static PutawayTaskId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
