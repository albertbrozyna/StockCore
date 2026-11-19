using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct DocumentNumberSequenceId(Guid Value) : IStronglyTypedId<Guid>
{
    public static DocumentNumberSequenceId New() => new(Guid.CreateVersion7());

    public static DocumentNumberSequenceId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
