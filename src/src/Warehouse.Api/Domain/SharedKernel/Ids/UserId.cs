using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct UserId(Guid Value) : IStronglyTypedId<Guid>
{
    public static UserId New() => new(Guid.CreateVersion7());

    public static UserId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
