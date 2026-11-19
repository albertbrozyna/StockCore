using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Api.Domain.Catalog;

/// <summary>Barcode of a SKU, optionally bound to a packaging unit. Managed through <see cref="Sku"/>.</summary>
public sealed class SkuBarcode : TenantEntity<SkuBarcodeId>
{
    private SkuBarcode()
    {
        Barcode = null!;
        Unit = null!;
    }

    public SkuId SkuId { get; private set; }

    public Barcode Barcode { get; private set; }

    /// <summary>The packaging unit this code identifies.</summary>
    public UnitCode Unit { get; private set; }

    public bool IsPrimary { get; private set; }

    internal static SkuBarcode Create(SkuId skuId, Barcode barcode, UnitCode unit, bool isPrimary) =>
        new()
        {
            Id = SkuBarcodeId.New(),
            SkuId = skuId,
            Barcode = barcode,
            Unit = unit,
            IsPrimary = isPrimary,
        };

    internal void SetPrimary(bool isPrimary) => IsPrimary = isPrimary;
}
