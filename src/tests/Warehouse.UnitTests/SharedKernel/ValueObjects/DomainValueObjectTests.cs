using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.UnitTests.SharedKernel.ValueObjects;

public sealed class DomainValueObjectTests
{
    [Fact]
    public void Codes_are_trimmed_normalized_and_length_limited()
    {
        SkuCode.Create(" abc-1 ").Value.ShouldBe("ABC-1");
        WarehouseCode.Create("wh1").Value.ShouldBe("WH1");
        LotNumber.Create(" l-1 ").Value.ShouldBe("l-1");
        LocationCode.Create("A-01-03-2").Value.ShouldBe("A-01-03-2");
        SerialNumber.Create(new string('x', 100)).Value.Length.ShouldBe(100);

        Should.Throw<DomainException>(() => SkuCode.Create(new string('a', 51)));
        Should.Throw<DomainException>(() => LotNumber.Create(new string('a', 51)));
        Should.Throw<DomainException>(() => SerialNumber.Create(new string('a', 101)));
        Should.Throw<DomainException>(() => LocationCode.Create(new string('a', 41)));
        Should.Throw<DomainException>(() => WarehouseCode.Create("ABCDEFGHIJK"));
        Should.Throw<DomainException>(() => SkuCode.Create(" "));
    }

    [Theory]
    [InlineData("4006381333931", BarcodeType.Ean13)]
    [InlineData("96385074", BarcodeType.Ean8)]
    [InlineData("036000291452", BarcodeType.UpcA)]
    public void Barcode_accepts_valid_check_digit(string value, BarcodeType type) =>
        Barcode.Create(value, type).Value.ShouldBe(value);

    [Theory]
    [InlineData("4006381333932", BarcodeType.Ean13)]
    [InlineData("96385075", BarcodeType.Ean8)]
    [InlineData("036000291453", BarcodeType.UpcA)]
    [InlineData("123", BarcodeType.Ean13)]
    [InlineData("40063813339AB", BarcodeType.Ean13)]
    public void Barcode_rejects_invalid_check_digit_or_length(string value, BarcodeType type) =>
        Should.Throw<DomainException>(() => Barcode.Create(value, type)).Error.Code.ShouldBe("Common.InvalidBarcode");

    [Fact]
    public void Barcode_without_checksum_types_accepts_any_text()
    {
        Barcode.Create("ANY-123", BarcodeType.Code128).Type.ShouldBe(BarcodeType.Code128);
        Barcode.Create("x", BarcodeType.Custom).Value.ShouldBe("x");
    }

    [Fact]
    public void TenantCode_matches_slug_pattern()
    {
        TenantCode.Create("acme-1").Value.ShouldBe("acme-1");
        Should.Throw<DomainException>(() => TenantCode.Create("ab"));
        Should.Throw<DomainException>(() => TenantCode.Create("Acme"));
        Should.Throw<DomainException>(() => TenantCode.Create(new string('a', 41)));
    }

    [Fact]
    public void DocumentNumber_is_formatted_with_padding()
    {
        var number = DocumentNumber.Create("PZ/WH1", 2026, 123, 6);

        number.Full.ShouldBe("PZ/WH1/2026/000123");
        number.Prefix.ShouldBe("PZ/WH1");
        number.Sequence.ShouldBe(123);
        Should.Throw<DomainException>(() => DocumentNumber.Create("PZ", 2026, 0, 6));
        Should.Throw<DomainException>(() => DocumentNumber.Create("PZ", 2026, 1, 0));
    }

    [Fact]
    public void ShelfLife_and_capacity_are_validated()
    {
        ShelfLife.Create(365, 90).MinRemainingDaysOnReceipt.ShouldBe(90);
        Should.Throw<DomainException>(() => ShelfLife.Create(0, 0));
        Should.Throw<DomainException>(() => ShelfLife.Create(10, 11));
        LocationCapacity.Create(100m, null, null, 2).MaxPallets.ShouldBe(2);
        LocationCapacity.Unlimited.MaxWeightKg.ShouldBeNull();
        Should.Throw<DomainException>(() => LocationCapacity.Create(-1m, null, null, null));
    }

    [Fact]
    public void ExternalReference_requires_source_and_value()
    {
        ExternalReference.Create("ERP-ORDER", "ZK/12/2026").Value.ShouldBe("ZK/12/2026");
        Should.Throw<DomainException>(() => ExternalReference.Create("", "x"));
    }

    [Fact]
    public void Tenant_settings_have_sensible_defaults()
    {
        var s = TenantSettings.Default;

        s.AllowNegativeStock.ShouldBeFalse();
        s.DefaultCurrency.Value.ShouldBe("PLN");
        s.Numbering.SequencePadding.ShouldBe(6);
        s.Numbering.ResetYearly.ShouldBeTrue();
    }
}
