using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.UnitTests.SharedKernel.ValueObjects;

public sealed class GeneralValueObjectTests
{
    private static readonly UnitCode Pcs = UnitCode.Create("pcs");

    [Fact]
    public void UnitCode_is_trimmed_upper_and_length_limited()
    {
        UnitCode.Create(" pcs ").Value.ShouldBe("PCS");
        UnitCode.Create("pcs").ShouldBe(Pcs);
        Should.Throw<DomainException>(() => UnitCode.Create("ABCDEFGHIJK"));
        Should.Throw<DomainException>(() => UnitCode.Create(" "));
    }

    [Fact]
    public void Quantity_arithmetic_works_within_one_unit()
    {
        var a = Quantity.Create(5m, Pcs);
        var b = Quantity.Create(2m, Pcs);

        a.Add(b).Amount.ShouldBe(7m);
        a.Subtract(b).Amount.ShouldBe(3m);
        b.Subtract(a).IsPositive.ShouldBeFalse();
        a.Negate().Amount.ShouldBe(-5m);
        a.Multiply(1.5m).Amount.ShouldBe(7.5m);
        Quantity.Zero(Pcs).IsZero.ShouldBeTrue();
    }

    [Fact]
    public void Quantity_rejects_different_units()
    {
        var ex = Should.Throw<DomainException>(() => Quantity.Create(1m, Pcs).Add(Quantity.Create(1m, UnitCode.Create("kg"))));

        ex.Error.Code.ShouldBe("Common.UnitMismatch");
    }

    [Fact]
    public void CurrencyCode_validates_against_known_list()
    {
        CurrencyCode.Create("pln").Value.ShouldBe("PLN");
        foreach (var code in new[] { "PLN", "EUR", "USD", "GBP", "CZK", "CHF" })
        {
            CurrencyCode.Create(code).Value.ShouldBe(code);
        }

        Should.Throw<DomainException>(() => CurrencyCode.Create("XXX"));
        Should.Throw<DomainException>(() => CurrencyCode.Create("PL"));
    }

    [Fact]
    public void Money_arithmetic_requires_same_currency()
    {
        var pln = CurrencyCode.Create("PLN");
        var a = Money.Create(10m, pln);

        a.Add(Money.Create(2m, pln)).Amount.ShouldBe(12m);
        a.Subtract(Money.Create(2m, pln)).Amount.ShouldBe(8m);
        a.Multiply(3m).Amount.ShouldBe(30m);
        Money.Zero(pln).Amount.ShouldBe(0m);
        Should.Throw<DomainException>(() => a.Add(Money.Create(1m, CurrencyCode.Create("EUR"))))
            .Error.Code.ShouldBe("Common.CurrencyMismatch");
    }

    [Fact]
    public void Dimensions_volume_and_weight_conversion()
    {
        Dimensions.Create(2m, 3m, 4m, LengthUnit.Cm).Volume.ShouldBe(24m);
        Should.Throw<DomainException>(() => Dimensions.Create(-1m, 1m, 1m, LengthUnit.Cm));
        Weight.Create(1500m, WeightUnit.G).ToKilograms().ShouldBe(1.5m);
        Weight.Create(2m, WeightUnit.Kg).ToKilograms().ShouldBe(2m);
    }

    [Fact]
    public void DateRange_requires_from_not_after_to()
    {
        var d = new DateOnly(2026, 1, 1);

        DateRange.Create(d, d).From.ShouldBe(d);
        Should.Throw<DomainException>(() => DateRange.Create(d.AddDays(1), d));
    }

    [Fact]
    public void Address_normalizes_and_validates()
    {
        var a = Address.Create("Prosta", "1", " ", "00-001", "Warszawa", null, "pl");

        a.CountryCode.ShouldBe("PL");
        a.ApartmentNo.ShouldBeNull();
        Should.Throw<DomainException>(() => Address.Create("", "1", null, "00-001", "W", null, "PL"));
        Should.Throw<DomainException>(() => Address.Create("S", "1", null, "00-001", "W", null, "POL"));
    }

    [Fact]
    public void ContactInfo_validates_email()
    {
        ContactInfo.Create("a@b.pl", null, "Jan").Email.ShouldBe("a@b.pl");
        ContactInfo.Create(null, "  ", null).Phone.ShouldBeNull();
        Should.Throw<DomainException>(() => ContactInfo.Create("not-an-email", null, null));
    }

    [Theory]
    [InlineData("5260250274")]
    [InlineData("526-025-02-74")]
    [InlineData("PL5260250274")]
    public void TaxId_accepts_valid_polish_nip(string nip) =>
        TaxId.Create("pl", nip).Number.ShouldBe("5260250274");

    [Theory]
    [InlineData("5260250275")]
    [InlineData("123")]
    [InlineData("ABCDEFGHIJ")]
    public void TaxId_rejects_invalid_polish_nip(string nip) =>
        Should.Throw<DomainException>(() => TaxId.Create("PL", nip)).Error.Code.ShouldBe("Common.InvalidTaxId");

    [Fact]
    public void TaxId_for_other_countries_only_needs_a_number()
    {
        TaxId.Create("DE", "DE123").Number.ShouldBe("DE123");
        Should.Throw<DomainException>(() => TaxId.Create("DE", " "));
    }
}
