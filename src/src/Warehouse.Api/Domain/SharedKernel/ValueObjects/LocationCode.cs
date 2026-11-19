namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Location code (e.g. <c>A-01-03-2</c>): trimmed, at most 40 characters.</summary>
public sealed record LocationCode
{
    public const int MaxLength = 40;

    private LocationCode(string value) => Value = value;

    public string Value { get; }

    public static LocationCode Create(string value)
    {
        var text = ValueObjectErrors.BoundedText(value, MaxLength, "Common.InvalidLocationCode", "LocationCode");
        return new LocationCode(text);
    }

    public override string ToString() => Value;
}
