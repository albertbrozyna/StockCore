using Warehouse.Domain.Catalog.Events;
using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Domain.Catalog;

/// <summary>A stock-keeping unit. Stock and ledger are always kept in <see cref="BaseUnit"/>.</summary>
public sealed class Sku : SoftDeletableTenantEntity<SkuId>, IAggregateRoot
{
    private readonly List<SkuBarcode> _barcodes = [];
    private readonly List<SkuUnitConversion> _unitConversions = [];
    private Dictionary<string, string> _attributes = [];

    private Sku()
    {
        Code = null!;
        Name = null!;
        BaseUnit = null!;
    }

    public SkuCode Code { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public SkuCategoryId? CategoryId { get; private set; }

    /// <summary>Base unit; there is no way to change it after creation (it is fixed once stock exists).</summary>
    public UnitCode BaseUnit { get; private set; }

    public TrackingMode TrackingMode { get; private set; }

    public bool RequiresExpiryDate { get; private set; }

    public ShelfLife? ShelfLife { get; private set; }

    public Dimensions? Dimensions { get; private set; }

    public Weight? Weight { get; private set; }

    /// <summary><c>null</c> = tenant default.</summary>
    public AllocationStrategyType? AllocationStrategy { get; private set; }

    /// <summary><c>null</c> = tenant default.</summary>
    public ValuationMethod? ValuationMethod { get; private set; }

    public decimal? ReorderPoint { get; private set; }

    public decimal? MinStockLevel { get; private set; }

    public ContractorId? DefaultSupplierId { get; private set; }

    public SkuId? VariantOfSkuId { get; private set; }

    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<SkuBarcode> Barcodes => _barcodes;

    public IReadOnlyCollection<SkuUnitConversion> UnitConversions => _unitConversions;

    public static Sku Create(
        TenantId tenantId,
        SkuCode code,
        string name,
        UnitCode baseUnit,
        TrackingMode trackingMode,
        DateTimeOffset now,
        string? description = null,
        SkuCategoryId? categoryId = null,
        bool requiresExpiryDate = false,
        ShelfLife? shelfLife = null,
        Dimensions? dimensions = null,
        Weight? weight = null,
        AllocationStrategyType? allocationStrategy = null,
        ValuationMethod? valuationMethod = null,
        decimal? reorderPoint = null,
        decimal? minStockLevel = null,
        ContractorId? defaultSupplierId = null,
        SkuId? variantOfSkuId = null,
        IReadOnlyDictionary<string, string>? attributes = null)
    {
        EnsureExpiryAllowed(requiresExpiryDate, trackingMode);
        var sku = new Sku
        {
            Id = SkuId.New(),
            Code = Guard.AgainstNull(code),
            Name = CatalogText.Required(name, 200, "Catalog.InvalidName", nameof(name)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            CategoryId = categoryId,
            BaseUnit = Guard.AgainstNull(baseUnit),
            TrackingMode = trackingMode,
            RequiresExpiryDate = requiresExpiryDate,
            ShelfLife = shelfLife,
            Dimensions = dimensions,
            Weight = weight,
            AllocationStrategy = allocationStrategy,
            ValuationMethod = valuationMethod,
            ReorderPoint = reorderPoint is { } rp ? Guard.AgainstNegative(rp) : null,
            MinStockLevel = minStockLevel is { } ms ? Guard.AgainstNegative(ms) : null,
            DefaultSupplierId = defaultSupplierId,
            VariantOfSkuId = variantOfSkuId,
            _attributes = attributes is null ? [] : new Dictionary<string, string>(attributes),
            IsActive = true,
        };
        sku.SetTenant(tenantId);
        sku.Raise(new SkuCreated(tenantId, now, sku.Id, sku.Code.Value));
        return sku;
    }

    public void Rename(string name) => Name = CatalogText.Required(name, 200, "Catalog.InvalidName", nameof(name));

    /// <summary>Changes tracking mode. The caller states whether any stock exists; if so the change is rejected.</summary>
    public void ChangeTrackingMode(TrackingMode trackingMode, bool hasStock)
    {
        if (hasStock)
        {
            throw CatalogText.Fail("Catalog.TrackingModeLocked", "Tracking mode cannot change while stock exists for the SKU.", ErrorType.Conflict);
        }

        EnsureExpiryAllowed(RequiresExpiryDate, trackingMode);
        TrackingMode = trackingMode;
    }

    public void AddBarcode(Barcode barcode, UnitCode unit, bool isPrimary = false)
    {
        Guard.AgainstNull(barcode);
        Guard.AgainstNull(unit);
        if (_barcodes.Exists(b => b.Barcode.Value == barcode.Value))
        {
            throw CatalogText.Fail("Catalog.DuplicateBarcode", $"Barcode '{barcode.Value}' already exists on this SKU.", ErrorType.Conflict);
        }

        if (isPrimary)
        {
            _barcodes.ForEach(b => b.SetPrimary(false));
        }

        _barcodes.Add(SkuBarcode.Create(Id, barcode, unit, isPrimary));
    }

    public void RemoveBarcode(Barcode barcode)
    {
        _barcodes.Remove(FindBarcode(barcode));
    }

    public void SetPrimaryBarcode(Barcode barcode)
    {
        var target = FindBarcode(barcode);
        _barcodes.ForEach(b => b.SetPrimary(ReferenceEquals(b, target)));
    }

    public void AddUnitConversion(UnitCode fromUnit, decimal factor, bool isDefaultForReceipt = false, bool isDefaultForIssue = false)
    {
        Guard.AgainstNull(fromUnit);
        if (factor <= 0m)
        {
            throw CatalogText.Fail("Catalog.InvalidConversionFactor", "Conversion factor must be greater than zero.", ErrorType.Validation);
        }

        if (fromUnit == BaseUnit)
        {
            throw CatalogText.Fail("Catalog.ConversionFromBaseUnit", "The base unit cannot have a conversion to itself.", ErrorType.Validation);
        }

        if (_unitConversions.Exists(c => c.FromUnit == fromUnit))
        {
            throw CatalogText.Fail("Catalog.DuplicateConversion", $"A conversion from '{fromUnit}' already exists.", ErrorType.Conflict);
        }

        if (isDefaultForReceipt)
        {
            _unitConversions.ForEach(c => c.ClearDefaultForReceipt());
        }

        if (isDefaultForIssue)
        {
            _unitConversions.ForEach(c => c.ClearDefaultForIssue());
        }

        _unitConversions.Add(SkuUnitConversion.Create(Id, fromUnit, factor, isDefaultForReceipt, isDefaultForIssue));
    }

    public void RemoveUnitConversion(UnitCode fromUnit)
    {
        Guard.AgainstNull(fromUnit);
        var conversion = _unitConversions.Find(c => c.FromUnit == fromUnit)
            ?? throw CatalogText.Fail("Catalog.ConversionNotFound", $"No conversion from '{fromUnit}'.", ErrorType.NotFound);
        _unitConversions.Remove(conversion);
    }

    /// <summary>Converts a quantity in the base unit or a configured packaging unit to the base unit.</summary>
    public Quantity ConvertToBase(Quantity quantity)
    {
        Guard.AgainstNull(quantity);
        if (quantity.Unit == BaseUnit)
        {
            return quantity;
        }

        return Quantity.Create(quantity.Amount * FindConversion(quantity.Unit).Factor, BaseUnit);
    }

    /// <summary>Converts an amount of the base unit to <paramref name="target"/> (a packaging unit or the base unit).</summary>
    public Quantity ConvertFromBase(decimal baseAmount, UnitCode target)
    {
        Guard.AgainstNull(target);
        return target == BaseUnit
            ? Quantity.Create(baseAmount, BaseUnit)
            : Quantity.Create(baseAmount / FindConversion(target).Factor, target);
    }

    public void Activate() => IsActive = true;

    public void Deactivate(DateTimeOffset now)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        Raise(new SkuDeactivated(TenantId, now, Id, Code.Value));
    }

    private static void EnsureExpiryAllowed(bool requiresExpiryDate, TrackingMode trackingMode)
    {
        if (requiresExpiryDate && trackingMode is not (TrackingMode.Lot or TrackingMode.LotAndSerial))
        {
            throw CatalogText.Fail("Catalog.ExpiryRequiresLotTracking", "RequiresExpiryDate needs lot tracking (Lot or LotAndSerial).", ErrorType.Validation);
        }
    }

    private SkuBarcode FindBarcode(Barcode barcode)
    {
        Guard.AgainstNull(barcode);
        return _barcodes.Find(b => b.Barcode.Value == barcode.Value)
            ?? throw CatalogText.Fail("Catalog.BarcodeNotFound", $"Barcode '{barcode.Value}' not found on this SKU.", ErrorType.NotFound);
    }

    private SkuUnitConversion FindConversion(UnitCode unit) =>
        _unitConversions.Find(c => c.FromUnit == unit)
            ?? throw CatalogText.Fail("Catalog.NoUnitConversion", $"SKU '{Code}' has no conversion for unit '{unit}'.", ErrorType.Validation);
}
