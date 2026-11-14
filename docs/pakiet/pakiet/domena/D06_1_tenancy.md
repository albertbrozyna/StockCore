<!-- Fragment DOMENA.md (D06_1_tenancy). Pełny kontekst: ref/DOMENA.md -->

### 6.1 Kontekst: Tenancy

#### `Tenant` · `AuditableEntity<TenantId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `TenantCode` | unikalny slug |
| `Name` | `string` | nazwa firmy |
| `Status` | `TenantStatus` | |
| `TimeZone` | `string` | IANA (np. `Europe/Warsaw`) |
| `Settings` | `TenantSettings` | VO osadzony |
| `TaxId` | `TaxId?` | |
| `Address` | `Address?` | |
| `ActivatedAt` / `SuspendedAt` | `DateTimeOffset?` | |

Metody: `Create`, `Rename`, `UpdateSettings`, `Suspend(reason)`, `Activate`, `Deactivate`.
Zdarzenia: `TenantCreated`, `TenantSuspended`, `TenantActivated`.
Niezmienniki: `Code` niezmienne po utworzeniu; zawieszony tenant odrzuca komendy mutujące (egzekwowane w pipeline, nie w agregatach innych kontekstów).

#### `DocumentNumberSequence` · `TenantEntity<DocumentNumberSequenceId>`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `DocumentType` | `DocumentType` | |
| `WarehouseId` | `WarehouseId?` | `null` = numeracja globalna tenanta |
| `Year` | `int?` | `null` gdy brak resetu rocznego |
| `Prefix` | `string` | |
| `LastValue` | `long` | ostatnio wydany numer |

Metoda: `long Next()` — inkrementacja pod **blokadą wiersza** (`FOR UPDATE`) lub `xmin` + retry. Unikalny indeks `(TenantId, DocumentType, WarehouseId, Year)`.
