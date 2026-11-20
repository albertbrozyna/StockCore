using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct DocumentId(Guid Value) : IStronglyTypedId<Guid>
{
    public static DocumentId New() => new(Guid.CreateVersion7());

    public static DocumentId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
