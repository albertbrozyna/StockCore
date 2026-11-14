# T19 — EF — konfiguracje: Inventory + Documents

**Fala:** 7 · **Zależy od:** T10, T11, T12, T13, T17 · **Szacunek wyjścia:** ~12k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D12_persystencja.md`
- `domena/D06_4_inventory.md`
- `domena/D06_5_documents.md`
- RAPORT_T10, RAPORT_T11, RAPORT_T12, RAPORT_T13, RAPORT_T17
- **Raporty** zależności: `RAPORT_T10.md`, `RAPORT_T11.md`, `RAPORT_T12.md`, `RAPORT_T13.md`, `RAPORT_T17.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T19 — EF — konfiguracje: Inventory + Documents.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Configurations/Inventory/*Configuration.cs (StockMovement, StockLevel, Lot, SerialItem, Reservation+Lines+Allocations, CostLayer, SkuCostBalance)
- Configurations/Documents/*Configuration.cs (TPH: dokumenty i linie z dyskryminatorem DocumentType, kolumny sparse nullable)
- Dopisanie DbSetów

## Zakres i reguły szczegółowe

- StockMovement: `Sequence` jako sekwencja per tenant (opisz wybór: sequence globalna vs. tabela liczników) + indeks (TenantId, SkuId, Sequence); blokada UPDATE/DELETE triggerem SQL (dodaj w migracji T20 — tu tylko zapisz skrypt `Sql/immutable_ledger.sql`)
- StockLevel: unikalny indeks z `NULLS NOT DISTINCT` dla LotId; CHECK `on_hand >= 0`
- Kolekcje encji zależnych jako owned/HasMany z kaskadą

## Poza zakresem

- Migracje (T20)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T19.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~12k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T19b`).
