<!-- Fragment DOMENA.md (D06_3_locations). Pełny kontekst: ref/DOMENA.md -->

### 6.3 Kontekst: Locations

#### `Warehouse` · `SoftDeletableTenantEntity<WarehouseId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `WarehouseCode` | unikalny w tenancie |
| `Name` | `string` | |
| `Type` | `WarehouseType` | |
| `Address` | `Address?` | |
| `TimeZone` | `string?` | nadpisuje tenanta |
| `AllowNegativeStock` | `bool?` | `null` = z `TenantSettings` |
| `DefaultReceivingLocationId` | `LocationId?` | |
| `DefaultShippingLocationId` | `LocationId?` | |
| `DefaultQuarantineLocationId` | `LocationId?` | |
| `IsActive` | `bool` | |

Metody: `Create`, `Update`, `SetDefaultLocations`, `Activate`, `Deactivate` (zablokowane gdy niezerowy stan), `Delete`.
Zdarzenia: `WarehouseCreated`, `WarehouseDeactivated`.

#### `Location` · `SoftDeletableTenantEntity<LocationId>`, `IAggregateRoot`

Drzewo: `Warehouse → Zone → Aisle → Rack → Shelf → Bin` (jedna klasa, typ w `LocationType`).

| Właściwość | Typ | Opis |
|------------|-----|------|
| `WarehouseId` | `WarehouseId` | |
| `ParentId` | `LocationId?` | |
| `Code` | `LocationCode` | unikalny w `(TenantId, WarehouseId)` |
| `Name` | `string?` | |
| `Type` | `LocationType` | |
| `Path` | `string` | materialized path |
| `Level` | `int` | głębokość w drzewie |
| `IsPickable` | `bool` | wolno kompletować |
| `IsReceivable` | `bool` | wolno przyjmować |
| `IsStorable` | `bool` | wolno składować (lokalizacje „liściowe") |
| `AllowedStockStatuses` | `IReadOnlySet<StockStatus>` | np. `Quarantine` tylko w lokalizacji kwarantanny |
| `Capacity` | `LocationCapacity?` | |
| `Dimensions` | `Dimensions?` | |
| `PickSequence` | `int?` | kolejność w ścieżce kompletacji |
| `BlockedReason` | `string?` | `!= null` ⇒ lokalizacja zablokowana |
| `IsActive` | `bool` | |

Metody: `Create`, `Move(newParent)`, `Rename`, `Block(reason)`, `Unblock`, `Activate`, `Deactivate`, `Delete`.
Niezmienniki: brak cykli w drzewie; dziecko w tym samym `WarehouseId`; nie wolno zdezaktywować lokalizacji z niezerowym stanem; lokalizacja zablokowana nie przyjmuje ani nie wydaje.
