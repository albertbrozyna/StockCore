using Warehouse.Domain.SharedKernel.Enums;

namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Non-negative weight in grams or kilograms.</summary>
public sealed record Weight
{
    private Weight(decimal value, WeightUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    public decimal Value { get; }

    public WeightUnit Unit { get; }

    public static Weight Create(decimal value, WeightUnit unit) => new(Guard.AgainstNegative(value), unit);

    public decimal ToKilograms() => Unit == WeightUnit.G ? Value / 1000m : Value;
}
