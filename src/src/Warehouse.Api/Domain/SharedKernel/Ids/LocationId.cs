using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct LocationId(Guid Value) : IStronglyTypedId<Guid>
{
    public static LocationId New() => new(Guid.CreateVersion7());

    public static LocationId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
