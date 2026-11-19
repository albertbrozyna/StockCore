using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct TenantId(Guid Value) : IStronglyTypedId<Guid>
{
    public static TenantId New() => new(Guid.CreateVersion7());

    public static TenantId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
