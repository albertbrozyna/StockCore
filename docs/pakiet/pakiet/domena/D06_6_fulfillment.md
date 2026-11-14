<!-- Fragment DOMENA.md (D06_6_fulfillment). Pełny kontekst: ref/DOMENA.md -->

### 6.6 Kontekst: Fulfillment

#### `PickingOrder` · `TenantEntity<PickingOrderId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Number` | `string` | |
| `WarehouseId` | `WarehouseId` | |
| `Status` | `PickingStatus` | |
| `Priority` | `int` | |
| `ReservationId` | `ReservationId?` | |
| `IssueDocumentId` | `DocumentId?` | powstaje po zakończeniu kompletacji |
| `AssignedTo` | `UserId?` | |
| `ReleasedAt`, `StartedAt`, `CompletedAt` | `DateTimeOffset?` | |
| `Lines` | `IReadOnlyCollection<PickingLine>` | |

`PickingLine` (zależna): `SkuId`, `LotId?`, `LocationId FromLocationId`, `LocationId? StagingLocationId`, `decimal QuantityRequired`, `decimal QuantityPicked`, `PickingLineStatus Status`, `ReservationAllocationId?`, `int Sequence` (kolejność ścieżki wg `Location.PickSequence`), `IReadOnlyCollection<SerialItemId> SerialItemIds`.
Metody: `Create`, `Release`, `Assign(userId)`, `Start`, `ConfirmPick(lineId, qty, serials?)`, `ReportShort(lineId, qty, reason)`, `Complete`, `Cancel`.
Zdarzenia: `PickingReleased`, `PickingCompleted`, `PickShortageReported`.

#### `PutawayTask` · `TenantEntity<PutawayTaskId>`, `IAggregateRoot`

`WarehouseId`, `SkuId`, `LotId?`, `decimal Quantity`, `LocationId SourceLocationId` (strefa przyjęć), `LocationId? SuggestedLocationId` (z `IPutawayStrategy`), `LocationId? ActualLocationId`, `PutawayStatus Status`, `DocumentLineId SourceDocumentLineId`, `UserId? AssignedTo`, `DateTimeOffset? CompletedAt`.
Metody: `Create`, `Suggest(strategy)`, `Assign`, `Complete(actualLocationId)` (generuje `TransferIn/Out` wewnątrz magazynu), `Cancel`.
