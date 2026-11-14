<!-- Fragment DOMENA.md (D06_5_documents). Pełny kontekst: ref/DOMENA.md -->

### 6.5 Kontekst: Documents

#### `WarehouseDocument` (abstract) · `TenantEntity<DocumentId>`, `IAggregateRoot`

Strategia mapowania EF: **TPH** (jedna tabela `documents.documents`, dyskryminator `DocumentType`).

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Type` | `DocumentType` | **abstract**, dyskryminator |
| `Number` | `DocumentNumber?` | nadawany przy zatwierdzeniu |
| `Status` | `DocumentStatus` | |
| `WarehouseId` | `WarehouseId` | |
| `DocumentDate` | `DateOnly` | data księgowa |
| `Notes` | `string?` | |
| `ExternalReference` | `ExternalReference?` | |
| `ApprovedAt` / `ApprovedBy` | `DateTimeOffset?` / `UserId?` | |
| `CancelledAt` / `CancelledBy` / `CancellationReason` | `DateTimeOffset?` / `UserId?` / `string?` | |
| `Lines` | `IReadOnlyCollection<DocumentLine>` | |
| `CorrectedByDocumentIds` | `IReadOnlyCollection<DocumentId>` | |

Metody wspólne: `AddLine`, `UpdateLine`, `RemoveLine` (tylko `Draft`), `Approve(DocumentNumber, IClock)`, `Cancel(reason)`, `MarkCorrected(DocumentId)`;
metody abstrakcyjne/wirtualne: `protected abstract void ValidateForApproval()`, `public abstract IReadOnlyList<MovementDraft> BuildMovements()` — dokument opisuje, **jakie ruchy powstają**, a `IStockPostingService` je księguje.
Zdarzenia: `DocumentCreated`, `DocumentApproved`, `DocumentCancelled`.
Niezmienniki: edycja tylko w `Draft`; zatwierdzenie wymaga ≥ 1 linii i dodatnich ilości; po `Approved` dokument niemutowalny; `Approve` jest idempotentne względem `Idempotency-Key`.

#### `DocumentLine` (abstract) · `TenantEntity<DocumentLineId>`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `DocumentId` | `DocumentId` | |
| `LineNumber` | `int` | |
| `SkuId` | `SkuId` | |
| `Quantity` | `Quantity` | w jednostce wpisanej przez użytkownika |
| `BaseQuantity` | `Quantity` | przeliczone na jednostkę bazową (zapisywane przy dodaniu linii) |
| `Notes` | `string?` | |

#### Dokumenty konkretne

**`ReceiptDocument` (PZ)**

| Właściwość | Typ |
|------------|-----|
| `SupplierId` | `ContractorId` |
| `SupplierDocumentNumber` | `string?` |
| `PurchaseOrderReference` | `ExternalReference?` |
| `ReceivedAt` | `DateTimeOffset?` |

`ReceiptLine : DocumentLine` — `ToLocationId LocationId`, `Money UnitCost`, `string? LotNumber`, `DateOnly? ProductionDate`, `DateOnly? ExpiryDate`, `IReadOnlyCollection<SerialNumber> SerialNumbers`, `StockStatus TargetStatus` (domyślnie `Available`).
Reguły: `TrackingMode=Lot` ⇒ wymagany `LotNumber`; `Serial` ⇒ `SerialNumbers.Count == BaseQuantity.Amount`; `RequiresExpiryDate` ⇒ wymagany `ExpiryDate` i spełnione `MinRemainingDaysOnReceipt`.
Ruchy: `ReceiptExternal` (+) na `ToLocationId`.

**`IssueDocument` (WZ)**

| Właściwość | Typ |
|------------|-----|
| `CustomerId` | `ContractorId` |
| `SalesOrderReference` | `ExternalReference?` |
| `ShippingAddress` | `Address?` |
| `ReservationId` | `ReservationId?` |
| `PickingOrderId` | `PickingOrderId?` |

`IssueLine : DocumentLine` — `FromLocationId LocationId?` (puste ⇒ alokacja automatyczna), `LotId? LotId`, `AllocationStrategyType? Strategy`, `IReadOnlyCollection<SerialItemId> SerialItemIds`, `Money? UnitPrice`, `ReservationLineId? ReservationLineId`.
Ruchy: `IssueExternal` (−); koszt z `IValuationStrategy`.

**`TransferDocument` (MM)**

| Właściwość | Typ |
|------------|-----|
| `SourceWarehouseId` | `WarehouseId` (= `WarehouseId` bazowe) |
| `TargetWarehouseId` | `WarehouseId` |
| `Mode` | `TransferMode` |
| `ShippedAt` / `ReceivedAt` | `DateTimeOffset?` |

`TransferLine : DocumentLine` — `FromLocationId`, `ToLocationId LocationId?` (dla `TwoStep` ustalane przy przyjęciu), `LotId?`, `SerialItemIds`, `decimal ReceivedQuantity`.
Ruchy: `Immediate` → `TransferOut` + `TransferIn` (wspólny `TransferPairId`) w jednej transakcji; `TwoStep` → `TransferOut` + `InTransit++`, potem `ReceiveTransfer` → `TransferIn`. Dodatkowy status `InTransit` w `DocumentStatus` dla `TwoStep`.

**`InternalReceiptDocument` (PW)** — `InternalReceiptReason Reason`, `string? CostCenter`; `InternalReceiptLine : DocumentLine` — jak `ReceiptLine` bez dostawcy (`ToLocationId`, `UnitCost`, `LotNumber?`, `ExpiryDate?`, `SerialNumbers`).

**`InternalIssueDocument` (RW)** — `InternalIssueReason Reason`, `string? CostCenter`; `InternalIssueLine : DocumentLine` — `FromLocationId?`, `LotId?`, `SerialItemIds`, `Strategy?`.

**`CorrectionDocument` (KOR)**

| Właściwość | Typ |
|------------|-----|
| `CorrectedDocumentId` | `DocumentId` |
| `Reason` | `CorrectionReason` |
| `ReasonDescription` | `string?` |

`CorrectionLine : DocumentLine` — `DocumentLineId OriginalLineId`, `decimal QuantityDelta` (±), `Money? NewUnitCost`, `LotId? NewLotId`, `LocationId? NewLocationId`.
Ruchy: `CorrectionIncrease` / `CorrectionDecrease` (lub `Reversal` + nowy ruch). Nigdy nie modyfikuje oryginału.

> **Inwentaryzacja** nie ma własnej klasy dokumentu: zatwierdzenie `StockTake` generuje `InternalReceiptDocument` / `InternalIssueDocument` z `Reason = StocktakeAdjustment`.
