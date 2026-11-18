# RAPORT_T06 — Zdarzenia domenowe (wszystkie)
Status: DONE
Zbudowano: tak (0 warnings) · Testy: przeszły (4 nowe w `DomainEventsTests`; 73 jednostkowe łącznie)

## Pliki
- `Domain/SharedKernel/Abstractions/IIntegrationEvent.cs` — `interface IIntegrationEvent : IDomainEvent` (marker outboxa).
- `Domain/<Kontekst>/Events/*.cs` — 28 rekordów + pomocniczy `StockReservedLine`.
- `tests/Warehouse.UnitTests/SharedKernel/Events/DomainEventsTests.cs` — liczba zdarzeń, zgodność znacznika integracyjnego z D09, brak VO we właściwościach, EventId/TenantId/OccurredAt.

## Publiczne API
Każde zdarzenie: `sealed record X(TenantId TenantId, DateTimeOffset OccurredAt, <pola>) : DomainEvent(TenantId, OccurredAt) [, IIntegrationEvent]`. ✔ = `IIntegrationEvent`.
```text
Tenancy:      TenantCreated ✔, TenantSuspended ✔
Catalog:      SkuCreated ✔ (SkuId, string Code), SkuDeactivated ✔ (SkuId, string Code)
Locations:    WarehouseCreated ✔, WarehouseDeactivated ✔ (WarehouseId)
Documents:    DocumentCreated (DocumentId, DocumentType Type); DocumentApproved ✔ (DocumentId, DocumentType Type, string Number, WarehouseId, DateTimeOffset ApprovedAt);
              DocumentCancelled ✔ (DocumentId, string Reason)
Inventory:    StockMoved ✔ (StockMovementId MovementId, SkuId, LocationId, decimal QuantityDelta, MovementType Type); StockChanged (StockLevelId, decimal OnHand, decimal Reserved);
              NegativeStockDetected ✔ (SkuId, LocationId, decimal OnHand); StockReserved ✔ (ReservationId, IReadOnlyList<StockReservedLine> Lines);
              ReservationReleased/Expired/Fulfilled ✔ (ReservationId, string Reason); LotCreated/LotBlocked/LotExpired ✔ (LotId, SkuId)
Alerts:       LowStockDetected ✔ (SkuId, WarehouseId, decimal Available, decimal Threshold)
Fulfillment:  PickingReleased/PickingCompleted/PickShortageReported ✔ (PickingOrderId); PutawayCompleted (PutawayTaskId, LocationId ActualLocationId)
Stocktaking:  StockTakeStarted/Closed/Approved ✔ (StockTakeId); StockTakeVarianceDetected ✔ (StockTakeId, StockTakeLineId LineId, decimal Difference)
public sealed record StockReservedLine(SkuId SkuId, decimal Quantity, string Unit);   // Inventory/Events
```

## Odstępstwa i uwagi
- D09 liczy zdarzenia w grupach — faktycznie jest ich **28**.
- `LowStockDetected` D09 przypisuje „handlerowi `StockChanged`” bez kontekstu — umieszczono w **Alerts** (handler alertów; zależność tylko od SharedKernel).
- `StockReservedLine` — D09 podaje tylko `Lines`; przyjęto (SkuId, Quantity, Unit jako string) — **do potwierdzenia**.
- `DocumentType`/`MovementType` w zdarzeniach to **smart enums** (klasy `Enumeration`), nie VO; D09 mówi „Type”. Serializacja JSON outboxa wymaga konwertera — dodany w T17 (`EnumerationJsonConverterFactory`, zapis po `Name`).
- `IIntegrationEvent : IDomainEvent` (nie czysty marker), żeby outbox miał `EventId/OccurredAt/TenantId`.
- `Number` w `DocumentApproved` to `string` (pełny numer), bo zdarzenia nie używają VO.
- `OccurredAt` jest parametrem konstruktora (patrz RAPORT_T02) — agregat podaje czas z `IClock`/`now`.

## Niewykonane (TODO dla kontynuacji)
- Brak (handlery i dispatch poza zakresem).
