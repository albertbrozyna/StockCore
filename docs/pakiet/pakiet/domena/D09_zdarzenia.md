<!-- Fragment DOMENA.md (D09_zdarzenia). Pełny kontekst: ref/DOMENA.md -->

## 9. Zdarzenia domenowe i integracyjne

Wszystkie dziedziczą po `DomainEvent`. „Integracyjne" = dodatkowo publikowane do Service Bus przez outbox.

| Zdarzenie | Zgłaszane przez | Kluczowe pola | Integracyjne |
|-----------|-----------------|---------------|:---:|
| `TenantCreated` / `TenantSuspended` | `Tenant` | `TenantId` | ✔ |
| `SkuCreated` / `SkuDeactivated` | `Sku` | `SkuId`, `Code` | ✔ |
| `WarehouseCreated` / `WarehouseDeactivated` | `Warehouse` | `WarehouseId` | ✔ |
| `DocumentCreated` | `WarehouseDocument` | `DocumentId`, `Type` | |
| `DocumentApproved` | `WarehouseDocument` | `DocumentId`, `Type`, `Number`, `WarehouseId`, `ApprovedAt` | ✔ |
| `DocumentCancelled` | `WarehouseDocument` | `DocumentId`, `Reason` | ✔ |
| `StockMoved` | `IStockPostingService` (po zaksięgowaniu) | `MovementId`, `SkuId`, `LocationId`, `QuantityDelta`, `Type` | ✔ |
| `StockChanged` | `StockLevel` | `StockLevelId`, `OnHand`, `Reserved` | |
| `NegativeStockDetected` | `StockLevel` | `SkuId`, `LocationId`, `OnHand` | ✔ |
| `LowStockDetected` | handler `StockChanged` | `SkuId`, `WarehouseId`, `Available`, `Threshold` | ✔ |
| `StockReserved` | `Reservation` | `ReservationId`, `Lines` | ✔ |
| `ReservationReleased` / `ReservationExpired` / `ReservationFulfilled` | `Reservation` | `ReservationId`, `Reason` | ✔ |
| `LotCreated` / `LotBlocked` / `LotExpired` | `Lot` | `LotId`, `SkuId` | ✔ |
| `PickingReleased` / `PickingCompleted` / `PickShortageReported` | `PickingOrder` | `PickingOrderId` | ✔ |
| `PutawayCompleted` | `PutawayTask` | `PutawayTaskId`, `ActualLocationId` | |
| `StockTakeStarted` / `StockTakeClosed` / `StockTakeApproved` | `StockTake` | `StockTakeId` | ✔ |
| `StockTakeVarianceDetected` | `StockTake` | `StockTakeId`, `LineId`, `Difference` | ✔ |

**Dispatch**: zdarzenia zbierane w `Entity.DomainEvents`; `DomainEventsInterceptor` wysyła je **po** udanym `SaveChanges` w tej samej transakcji (handlery wewnętrzne), a zdarzenia integracyjne trafiają do `OutboxMessage` w tej samej transakcji.
