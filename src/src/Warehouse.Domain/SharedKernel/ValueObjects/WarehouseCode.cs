namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Warehouse code: trimmed, upper-case, at most 10 characters.</summary>
public sealed record WarehouseCode
{
    public const int MaxLength = 10;

    private WarehouseCode(string value) => Value = value;

    public string Value { get; }

    public static WarehouseCode Create(string value)
    {
        var text = ValueObjectErrors.BoundedText(value, MaxLength, "Common.InvalidWarehouseCode", "WarehouseCode");
        return new WarehouseCode(text.ToUpperInvariant());
    }

    public override string ToString() => Value;
}
