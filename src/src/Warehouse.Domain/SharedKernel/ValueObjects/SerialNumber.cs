namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Serial number: trimmed, at most 100 characters.</summary>
public sealed record SerialNumber
{
    public const int MaxLength = 100;

    private SerialNumber(string value) => Value = value;

    public string Value { get; }

    public static SerialNumber Create(string value)
    {
        var text = ValueObjectErrors.BoundedText(value, MaxLength, "Common.InvalidSerialNumber", "SerialNumber");
        return new SerialNumber(text);
    }

    public override string ToString() => Value;
}
