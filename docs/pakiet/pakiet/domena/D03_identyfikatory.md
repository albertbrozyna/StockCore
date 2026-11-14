<!-- Fragment DOMENA.md (D03_identyfikatory). Pełny kontekst: ref/DOMENA.md -->

## 3. Identyfikatory (strongly-typed IDs)

Wszystkie: `readonly record struct <Nazwa>(Guid Value) : IStronglyTypedId<Guid>` z `static New()` (`Guid.CreateVersion7()`), `Empty`, `ToString()`, konwerter EF (`ValueConverter`) i konwerter System.Text.Json.

`TenantId`, `UserId`, `SkuId`, `SkuCategoryId`, `ContractorId`, `UnitOfMeasureId`, `WarehouseId`, `LocationId`, `DocumentId`, `DocumentLineId`, `StockMovementId`, `StockLevelId`, `LotId`, `SerialItemId`, `ReservationId`, `ReservationLineId`, `ReservationAllocationId`, `CostLayerId`, `SkuCostBalanceId`, `PickingOrderId`, `PickingLineId`, `PutawayTaskId`, `StockTakeId`, `StockTakeLineId`, `AlertRuleId`, `AlertId`, `DocumentNumberSequenceId`, `SkuBarcodeId`, `SkuUnitConversionId`.

> `StockMovementId` — dodatkowo kolumna `Sequence` (`long`, rosnąca per tenant) do porządkowania ledgera.
