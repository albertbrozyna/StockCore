namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Lot number: trimmed, at most 50 characters.</summary>
public sealed record LotNumber
{
    public const int MaxLength = 50;

    private LotNumber(string value) => Value = value;

    public string Value { get; }

    public static LotNumber Create(string value)
    {
        var text = ValueObjectErrors.BoundedText(value, MaxLength, "Common.InvalidLotNumber", "LotNumber");
        return new LotNumber(text);
    }

    public override string ToString() => Value;
}
