<!-- Fragment DOMENA.md (D05_enumeracje). Pełny kontekst: ref/DOMENA.md -->

## 5. Enumeracje

### 5.1 Smart enums (`Enumeration`) — z zachowaniem

**`DocumentType`**

| Id | Name | Skrót | Klasa dokumentu | Efekt na stan |
|----|------|-------|-----------------|---------------|
| 1 | `Receipt` | PZ | `ReceiptDocument` | + |
| 2 | `Issue` | WZ | `IssueDocument` | − |
| 3 | `Transfer` | MM | `TransferDocument` | − / + |
| 4 | `InternalReceipt` | PW | `InternalReceiptDocument` | + |
| 5 | `InternalIssue` | RW | `InternalIssueDocument` | − |
| 6 | `Correction` | KOR | `CorrectionDocument` | ± |

Właściwości: `string Code`, `bool RequiresApproval`, `bool AffectsStock`.

**`MovementType`**

| Name | `Direction` | `AffectsValuation` | Źródło |
|------|-------------|--------------------|--------|
| `ReceiptExternal` | In | true | PZ |
| `IssueExternal` | Out | true | WZ |
| `TransferOut` | Out | false | MM |
| `TransferIn` | In | false | MM |
| `ReceiptInternal` | In | true | PW |
| `IssueInternal` | Out | true | RW |
| `StocktakeSurplus` | In | true | INW → PW |
| `StocktakeShortage` | Out | true | INW → RW |
| `CorrectionIncrease` | In | true | KOR |
| `CorrectionDecrease` | Out | true | KOR |
| `Reversal` | In/Out | true | storno ruchu |

Właściwości: `MovementDirection Direction`, `bool AffectsValuation`, `Sign` (+1/−1).

**`DocumentStatus`** — z metodą `bool CanTransitionTo(DocumentStatus next)`:

`Draft` → `Approved` → (`Completed` tylko MM dwuetapowe) ; `Draft` → `Cancelled`. Zatwierdzony nie jest edytowalny ani anulowalny — tylko korygowany `KOR`.

### 5.2 Zwykłe enumy C#

| Enum | Wartości |
|------|----------|
| `MovementDirection` | `In`, `Out` |
| `TrackingMode` | `None`, `Lot`, `Serial`, `LotAndSerial` |
| `AllocationStrategyType` | `Fifo`, `Fefo`, `Lifo`, `SpecificLot` |
| `ValuationMethod` | `WeightedAverage`, `Fifo` |
| `StockStatus` | `Available`, `Quarantine`, `Blocked`, `Damaged` |
| `LotStatus` | `Available`, `Quarantine`, `Blocked`, `Expired` |
| `SerialStatus` | `InStock`, `Reserved`, `Issued`, `Returned`, `Scrapped` |
| `LocationType` | `Zone`, `Aisle`, `Rack`, `Shelf`, `Bin`, `Receiving`, `Shipping`, `Staging`, `Quarantine`, `Damaged`, `Transit`, `Virtual` |
| `WarehouseType` | `Standard`, `Transit`, `Virtual`, `Consignment` |
| `ReservationType` | `Soft`, `Hard` |
| `ReservationStatus` | `Active`, `PartiallyFulfilled`, `Fulfilled`, `Released`, `Expired`, `Cancelled` |
| `PickingStatus` | `Draft`, `Released`, `InProgress`, `Completed`, `Cancelled` |
| `PickingLineStatus` | `Pending`, `Picked`, `Short`, `Cancelled` |
| `PutawayStatus` | `Pending`, `Assigned`, `Completed`, `Cancelled` |
| `StockTakeType` | `Full`, `Cycle`, `Partial` |
| `StockTakeStatus` | `Planned`, `InProgress`, `CountingClosed`, `UnderReview`, `Approved`, `Cancelled` |
| `StockTakeLineStatus` | `Pending`, `Counted`, `RecountRequested`, `Accepted` |
| `TransferMode` | `Immediate`, `TwoStep` |
| `InternalReceiptReason` | `Production`, `Surplus`, `StocktakeAdjustment`, `ReturnFromCustomer`, `Other` |
| `InternalIssueReason` | `Consumption`, `Damage`, `Expiry`, `Loss`, `OwnUse`, `StocktakeAdjustment`, `Other` |
| `CorrectionReason` | `QuantityError`, `CostError`, `WrongLot`, `WrongLocation`, `Other` |
| `TenantStatus` | `Active`, `Suspended`, `Deactivated` |
| `ContractorKind` (flags) | `Supplier = 1`, `Customer = 2` |
| `AlertType` | `LowStock`, `ExpiryApproaching`, `StocktakeVariance`, `NegativeStock` |
| `AlertStatus` | `Open`, `Acknowledged`, `Resolved` |
| `BarcodeType` | `Ean13`, `Ean8`, `UpcA`, `Code128`, `Gs1128`, `Custom` |
| `LengthUnit` / `WeightUnit` | `Mm, Cm, M` / `G, Kg` |
| `UnitKind` | `Count`, `Weight`, `Volume`, `Length`, `Other` |
