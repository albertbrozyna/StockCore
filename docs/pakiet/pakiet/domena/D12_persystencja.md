<!-- Fragment DOMENA.md (D12_persystencja). Pełny kontekst: ref/DOMENA.md -->

## 12. Wskazówki persystencji (dla implementatora EF Core / PostgreSQL)

| Temat | Wskazówka |
|-------|-----------|
| Schematy | `catalog`, `locations`, `inventory`, `documents`, `fulfillment`, `stocktaking`, `alerts`, `tenancy`, `platform` (outbox, idempotency, audit) |
| Dziedziczenie dokumentów | TPH, dyskryminator `DocumentType`; linie również TPH (`DocumentLine`) |
| Value Objects | `OwnsOne` / `HasConversion`; `Quantity` jako 2 kolumny (`amount`, `unit`); `Money` jako 2 kolumny |
| Strongly-typed IDs | globalne konwertery (`ConfigureConventions`) |
| Współbieżność | `Version` → `xmin` (`IsRowVersion`); retry na `DbUpdateConcurrencyException` dla `StockLevel` |
| Blokady | `FOR UPDATE` na `StockLevel` i `DocumentNumberSequence` przy księgowaniu |
| Filtry globalne | `TenantId == ITenantContext.TenantId` na wszystkim z `IHasTenant`; `!IsDeleted` na `ISoftDeletable` |
| Indeksy | zawsze `(TenantId, …)`; unikalne: `Sku(TenantId, Code)`, `Location(TenantId, WarehouseId, Code)`, `Lot(TenantId, SkuId, LotNumber)`, `SerialItem(TenantId, SkuId, SerialNumber)`, `StockLevel(TenantId, LocationId, SkuId, LotId, StockStatus)` |
| Ledger | partycjonowanie `StockMovement` po `OccurredAt` (miesięcznie) — opcjonalnie od fazy 8 |
| JSONB | `Sku.Attributes`, `Lot.Attributes`, `AuditLogEntry.Changes`, `OutboxMessage.Payload` |
| Decimal | `numeric(18,4)`; `CHECK (on_hand >= 0)` tam, gdzie dozwolone |
| Row-Level Security | opcjonalnie RLS w Postgresie jako druga linia obrony multi-tenancy (ADR-007) |
