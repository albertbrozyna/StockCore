namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>ISO 4217 currency code (3 letters, upper-case) from a built-in list of common currencies.</summary>
public sealed record CurrencyCode
{
    private static readonly HashSet<string> Known = new(StringComparer.Ordinal)
    {
        "PLN", "EUR", "USD", "GBP", "CZK", "CHF", "SEK", "NOK", "DKK", "HUF", "RON", "BGN", "UAH", "JPY", "CAD", "AUD", "CNY",
    };

    private CurrencyCode(string value) => Value = value;

    public static CurrencyCode Pln { get; } = new("PLN");

    public string Value { get; }

    public static CurrencyCode Create(string value)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(value).Trim().ToUpperInvariant();
        return Known.Contains(normalized)
            ? new CurrencyCode(normalized)
            : throw ValueObjectErrors.Invalid("Common.InvalidCurrency", $"'{normalized}' is not a supported ISO 4217 currency code.");
    }

    public override string ToString() => Value;
}
