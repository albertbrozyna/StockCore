using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel.Ids;

public readonly record struct AlertRuleId(Guid Value) : IStronglyTypedId<Guid>
{
    public static AlertRuleId New() => new(Guid.CreateVersion7());

    public static AlertRuleId Empty => new(Guid.Empty);

    public bool IsEmpty => Value == Guid.Empty;

    public override string ToString() => Value.ToString();
}
