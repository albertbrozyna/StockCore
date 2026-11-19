using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct DocumentLineId(Guid Value) : IStronglyTypedId<Guid>
{
    public static DocumentLineId New() => new(Guid.CreateVersion7());

    public static DocumentLineId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
