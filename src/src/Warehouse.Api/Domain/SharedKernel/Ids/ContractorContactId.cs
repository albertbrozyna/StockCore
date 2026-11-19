using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct ContractorContactId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ContractorContactId New() => new(Guid.CreateVersion7());

    public static ContractorContactId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
