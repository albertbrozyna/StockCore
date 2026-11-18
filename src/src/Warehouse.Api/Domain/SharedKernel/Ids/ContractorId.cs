using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct ContractorId(Guid Value) : IStronglyTypedId<Guid>
{
    public static ContractorId New() => new(Guid.CreateVersion7());

    public static ContractorId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
