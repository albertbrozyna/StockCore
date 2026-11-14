<!-- Fragment DOMENA.md (D06_4_inventory). Pełny kontekst: ref/DOMENA.md -->

### 6.4 Kontekst: Inventory (rdzeń)

#### `StockMovement` · `ImmutableTenantEntity<StockMovementId>` — **dziennik (ledger)**

Wyłącznie `INSERT`. Brak update/delete (interceptor + trigger DB).

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Sequence` | `long` | monotoniczny numer w tenancie (`IDENTITY`/sekwencja) |
| `OccurredAt` | `DateTimeOffset` | moment zdarzenia biznesowego |
| `PostedAt` | `DateTimeOffset` | moment zaksięgowania (zwykle = `CreatedAt`) |
| `Type` | `MovementType` | |
| `WarehouseId` | `WarehouseId` | |
| `LocationId` | `LocationId` | |
| `SkuId` | `SkuId` | |
| `LotId` | `LotId?` | |
| `SerialItemId` | `SerialItemId?` | |
| `StockStatus` | `StockStatus` | status stanu, którego dotyczy ruch |
| `QuantityDelta` | `decimal` | **ze znakiem**, w jednostce bazowej Sku |
| `BaseUnit` | `UnitCode` | |
| `UnitCost` | `Money?` | koszt jednostkowy (jeśli `AffectsValuation`) |
| `TotalValue` | `Money?` | `QuantityDelta × UnitCost` |
| `DocumentId` | `DocumentId` | dokument-źródło |
| `DocumentLineId` | `DocumentLineId` | |
| `TransferPairId` | `Guid?` | wiąże `TransferOut` z `TransferIn` |
| `ReversesMovementId` | `StockMovementId?` | dla `Reversal` |
| `CorrelationId` | `Guid` | id żądania (do audytu i trace) |

Niezmienniki: `QuantityDelta != 0`; znak zgodny z `Type.Direction`; `Reversal` musi wskazywać `ReversesMovementId` i odwracać znak i wartość dokładnie.

#### `StockLevel` · `TenantEntity<StockLevelId>`, `IAggregateRoot` — **projekcja ledgera**

Jedna pozycja na kombinację `(Warehouse, Location, Sku, Lot?, StockStatus)`. Unikalny indeks `(TenantId, LocationId, SkuId, LotId, StockStatus)` (z `NULLS NOT DISTINCT`).

| Właściwość | Typ | Opis |
|------------|-----|------|
| `WarehouseId`, `LocationId`, `SkuId` | ID | |
| `LotId` | `LotId?` | |
| `StockStatus` | `StockStatus` | |
| `OnHand` | `decimal` | stan fizyczny |
| `Reserved` | `decimal` | zarezerwowane |
| `Available` | `decimal` | *computed* = `OnHand − Reserved` (kolumna generowana lub właściwość) |
| `InTransit` | `decimal` | do przyjęcia w MM dwuetapowym |
| `LastMovementSequence` | `long` | numer ostatniego zastosowanego ruchu (kontrola spójności/idempotentności) |
| `LastMovementAt` | `DateTimeOffset?` | |

Metody (wywoływane **wyłącznie** przez `IStockPostingService`): `Apply(StockMovement, bool allowNegative)`, `Reserve(decimal)`, `ReleaseReservation(decimal)`, `CommitReservation(decimal)`.
Niezmienniki: `OnHand >= 0` (chyba że `AllowNegativeStock`); `0 <= Reserved <= OnHand`; zastosowanie ruchu o `Sequence <= LastMovementSequence` jest odrzucane (idempotencja).
Zdarzenia: `StockChanged`, `NegativeStockDetected`.

#### `Lot` · `TenantEntity<LotId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `SkuId` | `SkuId` | |
| `LotNumber` | `LotNumber` | unikalny w `(TenantId, SkuId)` |
| `SupplierLotNumber` | `string?` | numer partii dostawcy |
| `SupplierId` | `ContractorId?` | |
| `ProductionDate` | `DateOnly?` | |
| `ExpiryDate` | `DateOnly?` | |
| `ReceivedAt` | `DateTimeOffset` | do FIFO |
| `Status` | `LotStatus` | |
| `Attributes` | `IReadOnlyDictionary<string,string>` | JSONB |

Metody: `Create`, `Block(reason)`, `Release`, `MoveToQuarantine`, `IsExpired(DateOnly today)`, `RemainingShelfLifeDays(DateOnly today)`.
Zdarzenia: `LotCreated`, `LotBlocked`, `LotExpired`.
Niezmienniki: `ExpiryDate >= ProductionDate`; partia wygasła/zablokowana nie jest alokowana do wydań.

#### `SerialItem` · `TenantEntity<SerialItemId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `SkuId` | `SkuId` | |
| `SerialNumber` | `SerialNumber` | unikalny w `(TenantId, SkuId)` |
| `LotId` | `LotId?` | |
| `Status` | `SerialStatus` | |
| `CurrentWarehouseId` / `CurrentLocationId` | ID? | `null` po wydaniu |
| `ReceivedDocumentLineId` | `DocumentLineId` | |
| `IssuedDocumentLineId` | `DocumentLineId?` | |
| `ReservationId` | `ReservationId?` | |

Metody: `Receive`, `MoveTo(locationId)`, `Reserve`, `Release`, `Issue`, `Return`, `Scrap`.
Niezmiennik: dozwolone przejścia statusów (np. `Issued` → tylko `Returned`); jeden numer seryjny nie może być równocześnie w dwóch lokalizacjach.

#### `Reservation` · `TenantEntity<ReservationId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Number` | `string` | |
| `WarehouseId` | `WarehouseId` | |
| `Type` | `ReservationType` | miękka (informacyjna) / twarda (blokuje stan) |
| `Status` | `ReservationStatus` | |
| `ExternalReference` | `ExternalReference?` | zamówienie sprzedaży itp. |
| `CustomerId` | `ContractorId?` | |
| `Priority` | `int` | |
| `ExpiresAt` | `DateTimeOffset?` | |
| `Lines` | `IReadOnlyCollection<ReservationLine>` | |
| `ReleasedAt` / `ReleaseReason` | `DateTimeOffset?` / `string?` | |

`ReservationLine` (zależna): `SkuId`, `Quantity RequestedQuantity`, `decimal AllocatedQuantity`, `decimal FulfilledQuantity`, `LotId? RequiredLotId`, `LocationId? PreferredLocationId`, `Allocations`.
`ReservationAllocation` (zależna od linii): `StockLevelId`, `LotId?`, `LocationId`, `decimal Quantity`, `decimal FulfilledQuantity`.

Metody: `Create`, `AddLine`, `Allocate(IStockAllocationStrategy, availability)`, `Release(reason)`, `Expire(now)`, `Fulfill(lineId, qty)`, `Cancel`.
Zdarzenia: `StockReserved`, `ReservationReleased`, `ReservationExpired`, `ReservationFulfilled`.
Niezmienniki: `Σ Allocation.Quantity ≤ RequestedQuantity`; twarda rezerwacja zwiększa `StockLevel.Reserved` **w tej samej transakcji**; rezerwacja nie przekracza `Available`.

#### `CostLayer` · `TenantEntity<CostLayerId>` — warstwa kosztowa FIFO

`SkuId`, `WarehouseId`, `LotId?`, `ReceivedAt`, `decimal OriginalQuantity`, `decimal RemainingQuantity`, `Money UnitCost`, `StockMovementId SourceMovementId`. Metoda: `Consume(decimal qty) → (consumed, cost)`.

#### `SkuCostBalance` · `TenantEntity<SkuCostBalanceId>` — saldo kosztowe (średnia ważona)

`SkuId`, `WarehouseId`, `decimal QuantityOnHand`, `Money TotalValue`, `Money AverageUnitCost` (computed). Metody: `AddReceipt(qty, unitCost)`, `ConsumeAtAverage(qty) → Money`.
