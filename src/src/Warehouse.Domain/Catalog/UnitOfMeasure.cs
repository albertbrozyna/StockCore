using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Domain.Catalog;

/// <summary>A unit of measure defined per tenant.</summary>
public sealed class UnitOfMeasure : SoftDeletableTenantEntity<UnitOfMeasureId>
{
    private const int MaxDecimalPlaces = 6;

    private UnitOfMeasure()
    {
        Code = null!;
        Name = null!;
    }

    public UnitCode Code { get; private set; }

    public string Name { get; private set; }

    public UnitKind Kind { get; private set; }

    /// <summary>Allowed precision (0 for pieces).</summary>
    public int DecimalPlaces { get; private set; }

    public static UnitOfMeasure Create(UnitCode code, string name, UnitKind kind, int decimalPlaces) =>
        new()
        {
            Id = UnitOfMeasureId.New(),
            Code = Guard.AgainstNull(code),
            Name = CatalogText.Required(name, 100, "Catalog.InvalidName", nameof(name)),
            Kind = kind,
            DecimalPlaces = Guard.AgainstOutOfRange(decimalPlaces, 0, MaxDecimalPlaces),
        };
}
