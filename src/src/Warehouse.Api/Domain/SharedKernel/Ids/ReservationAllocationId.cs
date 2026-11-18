using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct ReservationAllocationId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ReservationAllocationId New() => new(Guid.CreateVersion7());

    public static ReservationAllocationId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
