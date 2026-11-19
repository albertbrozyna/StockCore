using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Api.Domain.Catalog;

/// <summary>How many base units one <see cref="FromUnit"/> contains. Managed through <see cref="Sku"/>.</summary>
public sealed class SkuUnitConversion : TenantEntity<SkuUnitConversionId>
{
    private SkuUnitConversion()
    {
        FromUnit = null!;
    }

    public SkuId SkuId { get; private set; }

    public UnitCode FromUnit { get; private set; }

    /// <summary>Base units in one <see cref="FromUnit"/> (e.g. CTN → 12). Always positive.</summary>
    public decimal Factor { get; private set; }

    public bool IsDefaultForReceipt { get; private set; }

    public bool IsDefaultForIssue { get; private set; }

    internal static SkuUnitConversion Create(SkuId skuId, UnitCode fromUnit, decimal factor, bool isDefaultForReceipt, bool isDefaultForIssue) =>
        new()
        {
            Id = SkuUnitConversionId.New(),
            SkuId = skuId,
            FromUnit = fromUnit,
            Factor = factor,
            IsDefaultForReceipt = isDefaultForReceipt,
            IsDefaultForIssue = isDefaultForIssue,
        };

    internal void ClearDefaultForReceipt() => IsDefaultForReceipt = false;

    internal void ClearDefaultForIssue() => IsDefaultForIssue = false;
}
