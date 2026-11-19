namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Unit of measure code, e.g. <c>PCS</c>, <c>KG</c>: trimmed, upper-case, 1–10 characters.</summary>
public sealed record UnitCode
{
    public const int MaxLength = 10;

    private UnitCode(string value) => Value = value;

    public string Value { get; }

    public static UnitCode Create(string value)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(value).Trim().ToUpperInvariant();
        return normalized.Length > MaxLength
            ? throw ValueObjectErrors.Invalid("Common.InvalidUnitCode", $"Unit code must have 1-{MaxLength} characters.")
            : new UnitCode(normalized);
    }

    public override string ToString() => Value;
}
