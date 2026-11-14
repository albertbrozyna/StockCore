<!-- Fragment DOMENA.md (D13_otwarte_decyzje). Pełny kontekst: ref/DOMENA.md -->

## 13. Do rozstrzygnięcia przez architekta (nowe lub doprecyzowane ADR)

1. **Zdarzenia w `Entity<TId>` czy tylko w korzeniach agregatów** — tu: w bazie `Entity` dla prostoty; alternatywa: osobna klasa `AggregateRoot<TId>` (wymaga rozdzielenia gałęzi tenant/non-tenant).
2. **Granica agregatu `StockLevel` vs `Reservation`** — tu: rezerwacja aktualizuje `StockLevel.Reserved` w tej samej transakcji (świadome odstępstwo od „jeden agregat na transakcję").
3. **TPH vs TPT dla dokumentów** — TPH (szybsze zapytania), kosztem wielu kolumn nullable.
4. **Księgowanie**: synchroniczne w handlerze `ApproveDocument` (tu) vs asynchroniczne przez zdarzenie (większa złożoność, eventual consistency stanu).
5. **Multi-tenancy**: shared schema + `TenantId` (tu) vs schema-per-tenant vs db-per-tenant; potrzebne ADR + decyzja o RLS.
6. **Jednostki miary**: tenant-scoped `UnitOfMeasure` (tu) vs globalny słownik systemowy.
7. **Wersjonowanie `Sku`** (zmiana atrybutów w czasie) — czy wymagane historyzowanie poza audytem.
8. **Koszt FIFO + partie**: czy warstwa kosztowa jest per `(Sku, Warehouse)` czy per `Lot`.
9. **Uprawnienia**: role vs. polityki per magazyn (ADR-006) — wpływa na `ICurrentUser.HasPermission`.
