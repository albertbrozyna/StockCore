using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Domain.Catalog.Services;

/// <summary>Converts quantities between a SKU's packaging units and its base unit.</summary>
public interface IUnitConverter
{
    Quantity ToBase(Sku sku, Quantity quantity);

    Quantity FromBase(Sku sku, decimal baseAmount, UnitCode target);
}
