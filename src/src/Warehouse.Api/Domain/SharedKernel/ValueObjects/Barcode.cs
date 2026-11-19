using Warehouse.Api.Domain.SharedKernel.Enums;

namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Barcode value with its symbology. EAN-13, EAN-8 and UPC-A are checked by length and modulo-10 check digit.</summary>
public sealed record Barcode
{
    private Barcode(string value, BarcodeType type)
    {
        Value = value;
        Type = type;
    }

    public string Value { get; }

    public BarcodeType Type { get; }

    public static Barcode Create(string value, BarcodeType type)
    {
        var text = Guard.AgainstNullOrWhiteSpace(value).Trim();
        var length = type switch
        {
            BarcodeType.Ean13 => 13,
            BarcodeType.Ean8 => 8,
            BarcodeType.UpcA => 12,
            _ => 0,
        };

        if (length > 0 && !HasValidCheckDigit(text, length))
        {
            throw ValueObjectErrors.Invalid("Common.InvalidBarcode", $"'{text}' is not a valid {type} barcode.");
        }

        return new Barcode(text, type);
    }

    public override string ToString() => Value;

    private static bool HasValidCheckDigit(string digits, int length)
    {
        if (digits.Length != length || !digits.All(char.IsAsciiDigit))
        {
            return false;
        }

        // Weights 3,1,3,... applied right-to-left over the digits preceding the check digit.
        var sum = 0;
        for (var i = length - 2; i >= 0; i--)
        {
            var weight = (length - 2 - i) % 2 == 0 ? 3 : 1;
            sum += (digits[i] - '0') * weight;
        }

        return (10 - (sum % 10)) % 10 == digits[^1] - '0';
    }
}
