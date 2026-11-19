using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Api.Domain.Catalog.Services;

public sealed class UnitConverter : IUnitConverter
{
    public Quantity ToBase(Sku sku, Quantity quantity) => Guard.AgainstNull(sku).ConvertToBase(quantity);

    public Quantity FromBase(Sku sku, decimal baseAmount, UnitCode target) =>
        Guard.AgainstNull(sku).ConvertFromBase(baseAmount, target);
}
