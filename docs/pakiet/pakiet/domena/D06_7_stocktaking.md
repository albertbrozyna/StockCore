<!-- Fragment DOMENA.md (D06_7_stocktaking). Pełny kontekst: ref/DOMENA.md -->

### 6.7 Kontekst: Stocktaking

#### `StockTake` · `TenantEntity<StockTakeId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Number` | `string` | |
| `WarehouseId` | `WarehouseId` | |
| `Type` | `StockTakeType` | |
| `Status` | `StockTakeStatus` | |
| `ScopeLocationIds` | `IReadOnlyCollection<LocationId>` | pusty = cały magazyn |
| `ScopeSkuIds` | `IReadOnlyCollection<SkuId>` | pusty = wszystkie |
| `FreezeStock` | `bool` | blokuje ruchy w zakresie na czas liczenia |
| `PlannedFor` | `DateOnly?` | |
| `StartedAt`, `CountingClosedAt`, `ApprovedAt` | `DateTimeOffset?` | |
| `ApprovedBy` | `UserId?` | |
| `AdjustmentDocumentIds` | `IReadOnlyCollection<DocumentId>` | wygenerowane PW/RW |
| `Lines` | `IReadOnlyCollection<StockTakeLine>` | |

`StockTakeLine` (zależna): `LocationId`, `SkuId`, `LotId?`, `decimal SystemQuantity` (snapshot na `StartedAt`), `decimal? CountedQuantity`, `decimal? Difference` (computed), `UserId? CountedBy`, `DateTimeOffset? CountedAt`, `StockTakeLineStatus Status`, `string? Comment`, `int RecountNumber`.
Metody: `Plan`, `Start` (robi snapshot `StockLevel`), `RecordCount(lineId, qty, userId)`, `RequestRecount(lineId)`, `CloseCounting`, `Approve` (generuje korekty), `Cancel`.
Zdarzenia: `StockTakeStarted`, `StockTakeClosed`, `StockTakeApproved`, `StockTakeVarianceDetected`.
Niezmienniki: po `Approve` brak edycji; różnice liczone względem snapshotu **oraz** ruchów po `StartedAt` (gdy `FreezeStock=false`).
