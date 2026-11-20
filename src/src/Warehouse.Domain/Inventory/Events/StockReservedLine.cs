using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Inventory.Events;

/// <summary>Reserved quantity of one SKU in a <see cref="StockReserved"/> event (primitives and IDs only).</summary>
public sealed record StockReservedLine(SkuId SkuId, decimal Quantity, string Unit);
