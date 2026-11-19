using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct ContractorAddressId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ContractorAddressId New() => new(Guid.CreateVersion7());

    public static ContractorAddressId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
