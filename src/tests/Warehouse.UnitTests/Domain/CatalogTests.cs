using Warehouse.Domain.Catalog;
using Warehouse.Domain.Catalog.Events;
using Warehouse.Domain.Catalog.Services;
using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.UnitTests.Domain;

public sealed class CatalogTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);
    private static readonly UnitCode Pcs = UnitCode.Create("PCS");
    private static readonly UnitCode Ctn = UnitCode.Create("CTN");

    private static Sku NewSku(TrackingMode mode = TrackingMode.None, bool expiry = false) =>
        Sku.Create(TenantId.New(), SkuCode.Create("abc-1"), "Widget", Pcs, mode, Now, requiresExpiryDate: expiry);

    [Fact]
    public void Create_sets_tenant_and_raises_SkuCreated()
    {
        var tenant = TenantId.New();
        var sku = Sku.Create(tenant, SkuCode.Create("abc-1"), "Widget", Pcs, TrackingMode.None, Now);

        sku.TenantId.ShouldBe(tenant);
        sku.IsActive.ShouldBeTrue();
        var created = sku.DomainEvents.OfType<SkuCreated>().Single();
        created.TenantId.ShouldBe(tenant);
        created.Code.ShouldBe("ABC-1");
    }

    [Theory]
    [InlineData(TrackingMode.None)]
    [InlineData(TrackingMode.Serial)]
    public void Expiry_requires_lot_tracking(TrackingMode mode) =>
        Should.Throw<DomainException>(() => NewSku(mode, expiry: true))
            .Error.Code.ShouldBe("Catalog.ExpiryRequiresLotTracking");

    [Fact]
    public void ChangeTrackingMode_is_blocked_when_stock_exists()
    {
        var sku = NewSku();
        Should.Throw<DomainException>(() => sku.ChangeTrackingMode(TrackingMode.Lot, hasStock: true))
            .Error.Code.ShouldBe("Catalog.TrackingModeLocked");

        sku.ChangeTrackingMode(TrackingMode.Lot, hasStock: false);
        sku.TrackingMode.ShouldBe(TrackingMode.Lot);
    }

    [Fact]
    public void ChangeTrackingMode_cannot_break_expiry_rule()
    {
        var sku = NewSku(TrackingMode.Lot, expiry: true);
        Should.Throw<DomainException>(() => sku.ChangeTrackingMode(TrackingMode.None, hasStock: false));
    }

    [Fact]
    public void Only_one_barcode_is_primary()
    {
        var sku = NewSku();
        var a = Barcode.Create("4006381333931", BarcodeType.Ean13);
        var b = Barcode.Create("ABC-123", BarcodeType.Code128);
        sku.AddBarcode(a, Pcs, isPrimary: true);
        sku.AddBarcode(b, Pcs, isPrimary: true);

        sku.Barcodes.Count(x => x.IsPrimary).ShouldBe(1);
        sku.Barcodes.Single(x => x.IsPrimary).Barcode.ShouldBe(b);

        sku.SetPrimaryBarcode(a);
        sku.Barcodes.Single(x => x.IsPrimary).Barcode.ShouldBe(a);
    }

    [Fact]
    public void Duplicate_and_missing_barcodes_are_rejected()
    {
        var sku = NewSku();
        var a = Barcode.Create("ABC-123", BarcodeType.Code128);
        sku.AddBarcode(a, Pcs);

        Should.Throw<DomainException>(() => sku.AddBarcode(a, Ctn)).Error.Code.ShouldBe("Catalog.DuplicateBarcode");
        sku.RemoveBarcode(a);
        Should.Throw<DomainException>(() => sku.RemoveBarcode(a)).Error.Code.ShouldBe("Catalog.BarcodeNotFound");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Conversion_factor_must_be_positive(int factor) =>
        Should.Throw<DomainException>(() => NewSku().AddUnitConversion(Ctn, factor))
            .Error.Code.ShouldBe("Catalog.InvalidConversionFactor");

    [Fact]
    public void Conversion_rules_are_enforced()
    {
        var sku = NewSku();
        sku.AddUnitConversion(Ctn, 12, isDefaultForReceipt: true);

        Should.Throw<DomainException>(() => sku.AddUnitConversion(Pcs, 1)).Error.Code.ShouldBe("Catalog.ConversionFromBaseUnit");
        Should.Throw<DomainException>(() => sku.AddUnitConversion(Ctn, 6)).Error.Code.ShouldBe("Catalog.DuplicateConversion");

        sku.AddUnitConversion(UnitCode.Create("PAL"), 480, isDefaultForReceipt: true);
        sku.UnitConversions.Count(c => c.IsDefaultForReceipt).ShouldBe(1);

        sku.RemoveUnitConversion(Ctn);
        Should.Throw<DomainException>(() => sku.RemoveUnitConversion(Ctn)).Error.Code.ShouldBe("Catalog.ConversionNotFound");
    }

    [Fact]
    public void Converts_to_and_from_base_unit()
    {
        var sku = NewSku();
        sku.AddUnitConversion(Ctn, 12);
        var converter = new UnitConverter();

        var toBase = converter.ToBase(sku, Quantity.Create(2.5m, Ctn));
        toBase.Amount.ShouldBe(30m);
        toBase.Unit.ShouldBe(Pcs);
        converter.ToBase(sku, Quantity.Create(5m, Pcs)).Amount.ShouldBe(5m);

        var back = converter.FromBase(sku, 30m, Ctn);
        back.Amount.ShouldBe(2.5m);
        back.Unit.ShouldBe(Ctn);

        Should.Throw<DomainException>(() => sku.ConvertToBase(Quantity.Create(1m, UnitCode.Create("KG"))))
            .Error.Code.ShouldBe("Catalog.NoUnitConversion");
    }

    [Fact]
    public void Deactivate_raises_event_once()
    {
        var sku = NewSku();
        sku.Deactivate(Now);
        sku.Deactivate(Now);

        sku.IsActive.ShouldBeFalse();
        sku.DomainEvents.OfType<SkuDeactivated>().Count().ShouldBe(1);
    }

    [Fact]
    public void Category_path_is_materialized_from_parent()
    {
        var root = SkuCategory.Create("ROOT", "Root");
        var child = SkuCategory.Create("CH", "Child", root);

        root.Path.ShouldBe($"/{root.Id.Value:N}/");
        child.ParentId.ShouldBe(root.Id);
        child.Path.ShouldBe($"{root.Path}{child.Id.Value:N}/");
    }

    [Fact]
    public void UnitOfMeasure_validates_decimal_places()
    {
        UnitOfMeasure.Create(Pcs, "Pieces", UnitKind.Count, 0).DecimalPlaces.ShouldBe(0);
        Should.Throw<DomainException>(() => UnitOfMeasure.Create(Pcs, "Pieces", UnitKind.Count, 7));
    }

    [Fact]
    public void Contractor_has_single_default_address_per_type()
    {
        var contractor = Contractor.Create("C1", "ACME", ContractorKind.Supplier | ContractorKind.Customer);
        var address = Address.Create("Prosta", "1", null, "00-001", "Warszawa", null, "PL");
        contractor.AddAddress(address, AddressType.Shipping, isDefault: true);
        contractor.AddAddress(address, AddressType.Billing, isDefault: true);
        contractor.AddAddress(address, AddressType.Shipping, isDefault: true);

        contractor.Addresses.Count(a => a.IsDefault).ShouldBe(2);
        contractor.Addresses.Count(a => a.IsDefault && a.Type == AddressType.Shipping).ShouldBe(1);
    }

    [Fact]
    public void Contractor_requires_a_kind_and_rejects_duplicate_references()
    {
        Should.Throw<DomainException>(() => Contractor.Create("C1", "ACME", 0))
            .Error.Code.ShouldBe("Catalog.InvalidContractorKind");

        var contractor = Contractor.Create("C1", "ACME", ContractorKind.Supplier);
        contractor.AddContact(ContactInfo.Create("a@b.pl", null, "Jan"), "Sales");
        contractor.Contacts.Single().Role.ShouldBe("Sales");

        var reference = ExternalReference.Create("ERP", "42");
        contractor.AddExternalReference(reference);
        Should.Throw<DomainException>(() => contractor.AddExternalReference(reference));
    }
}
