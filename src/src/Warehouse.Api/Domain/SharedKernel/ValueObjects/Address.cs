namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Postal address. <see cref="CountryCode"/> is an upper-case ISO 3166 alpha-2 code.</summary>
public sealed record Address
{
    private Address(string street, string buildingNo, string? apartmentNo, string postalCode, string city, string? region, string countryCode)
    {
        Street = street;
        BuildingNo = buildingNo;
        ApartmentNo = apartmentNo;
        PostalCode = postalCode;
        City = city;
        Region = region;
        CountryCode = countryCode;
    }

    public string Street { get; }

    public string BuildingNo { get; }

    public string? ApartmentNo { get; }

    public string PostalCode { get; }

    public string City { get; }

    public string? Region { get; }

    public string CountryCode { get; }

    public static Address Create(
        string street,
        string buildingNo,
        string? apartmentNo,
        string postalCode,
        string city,
        string? region,
        string countryCode)
    {
        var country = Guard.AgainstNullOrWhiteSpace(countryCode).Trim().ToUpperInvariant();
        if (country.Length != 2 || !country.All(char.IsAsciiLetterUpper))
        {
            throw ValueObjectErrors.Invalid("Common.InvalidCountryCode", "Country code must be 2 letters (ISO 3166 alpha-2).");
        }

        return new Address(
            Guard.AgainstNullOrWhiteSpace(street).Trim(),
            Guard.AgainstNullOrWhiteSpace(buildingNo).Trim(),
            Normalize(apartmentNo),
            Guard.AgainstNullOrWhiteSpace(postalCode).Trim(),
            Guard.AgainstNullOrWhiteSpace(city).Trim(),
            Normalize(region),
            country);
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
