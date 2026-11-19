namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Tax identifier. For <c>PL</c> the number is a NIP validated by its checksum; other countries only require a non-empty number.</summary>
public sealed record TaxId
{
    private static readonly int[] NipWeights = [6, 5, 7, 2, 3, 4, 5, 6, 7];

    private TaxId(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    public string CountryCode { get; }

    public string Number { get; }

    public static TaxId Create(string countryCode, string number)
    {
        var country = Guard.AgainstNullOrWhiteSpace(countryCode).Trim().ToUpperInvariant();
        if (country.Length != 2 || !country.All(char.IsAsciiLetterUpper))
        {
            throw ValueObjectErrors.Invalid("Common.InvalidCountryCode", "Country code must be 2 letters (ISO 3166 alpha-2).");
        }

        var raw = Guard.AgainstNullOrWhiteSpace(number).Trim();
        if (country != "PL")
        {
            return new TaxId(country, raw);
        }

        var digits = new string(raw.Where(c => c is not ('-' or ' ')).ToArray());
        if (digits.StartsWith("PL", StringComparison.OrdinalIgnoreCase))
        {
            digits = digits[2..];
        }

        return IsValidNip(digits)
            ? new TaxId(country, digits)
            : throw ValueObjectErrors.Invalid("Common.InvalidTaxId", "Invalid Polish NIP.");
    }

    private static bool IsValidNip(string digits)
    {
        if (digits.Length != 10 || !digits.All(char.IsAsciiDigit))
        {
            return false;
        }

        var sum = 0;
        for (var i = 0; i < NipWeights.Length; i++)
        {
            sum += (digits[i] - '0') * NipWeights[i];
        }

        var check = sum % 11;
        return check != 10 && check == digits[9] - '0';
    }
}
