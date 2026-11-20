using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct ReservationLineId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ReservationLineId New() => new(Guid.CreateVersion7());

    public static ReservationLineId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
