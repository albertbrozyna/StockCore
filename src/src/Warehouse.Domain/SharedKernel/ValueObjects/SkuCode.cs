namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>SKU code: trimmed, upper-case, at most 50 characters.</summary>
public sealed record SkuCode
{
    public const int MaxLength = 50;

    private SkuCode(string value) => Value = value;

    public string Value { get; }

    public static SkuCode Create(string value)
    {
        var text = ValueObjectErrors.BoundedText(value, MaxLength, "Common.InvalidSkuCode", "SkuCode");
        return new SkuCode(text.ToUpperInvariant());
    }

    public override string ToString() => Value;
}
