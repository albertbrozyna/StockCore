namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Amount in a unit. Negative amounts are allowed here so the type can also express deltas.</summary>
public sealed record Quantity
{
    private Quantity(decimal amount, UnitCode unit)
    {
        Amount = amount;
        Unit = unit;
    }

    public decimal Amount { get; }

    public UnitCode Unit { get; }

    public bool IsZero => Amount == 0m;

    public bool IsPositive => Amount > 0m;

    public static Quantity Create(decimal amount, UnitCode unit) => new(amount, Guard.AgainstNull(unit));

    public static Quantity Zero(UnitCode unit) => Create(0m, unit);

    public Quantity Add(Quantity other) => new(Amount + EnsureSameUnit(other).Amount, Unit);

    public Quantity Subtract(Quantity other) => new(Amount - EnsureSameUnit(other).Amount, Unit);

    public Quantity Negate() => new(-Amount, Unit);

    public Quantity Multiply(decimal factor) => new(Amount * factor, Unit);

    private Quantity EnsureSameUnit(Quantity other)
    {
        Guard.AgainstNull(other);
        return other.Unit == Unit
            ? other
            : throw ValueObjectErrors.Invalid("Common.UnitMismatch", $"Cannot combine quantities in '{Unit}' and '{other.Unit}'.");
    }
}
