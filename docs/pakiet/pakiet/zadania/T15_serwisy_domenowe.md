# T15 — Serwisy domenowe — strategie i polityki

**Fala:** 6 · **Zależy od:** T08, T09, T10, T11 · **Szacunek wyjścia:** ~14k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D07_serwisy_domenowe.md`
- RAPORT_T08, RAPORT_T09, RAPORT_T10, RAPORT_T11 (sygnatury — załącz!)
- **Raporty** zależności: `RAPORT_T08.md`, `RAPORT_T09.md`, `RAPORT_T10.md`, `RAPORT_T11.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T15 — Serwisy domenowe — strategie i polityki.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Inventory/Allocation/: IStockAllocationStrategy, FifoAllocationStrategy, FefoAllocationStrategy, LifoAllocationStrategy, SpecificLotAllocationStrategy, AllocationCandidate, AllocationContext, AllocationResult
- Domain/Inventory/Valuation/: IValuationStrategy, WeightedAverageValuationStrategy, FifoValuationStrategy, ValuationState, ValuationResult
- Domain/Inventory/Services/: IStockAvailabilityPolicy + StockAvailabilityPolicy
- Domain/Catalog/Services/: IExpiryPolicy + ExpiryPolicy
- Domain/Documents/Services/: IDocumentNumberGenerator + DocumentNumberGenerator
- Domain/Fulfillment/Services/: IPutawayStrategy + FixedLocationPutaway, NearestEmptyPutaway, ByCategoryPutaway
- Domain/Stocktaking/Services/: IStockTakeVariancePolicy + implementacja

## Zakres i reguły szczegółowe

- Wszystko czyste (bez I/O), deterministyczne — dane przez parametry
- FEFO: sortuj po ExpiryDate, pomijaj partie Blocked/Expired/Quarantine; FIFO po Lot.ReceivedAt; LIFO odwrotnie
- Alokacja zwraca częściową alokację + brakującą ilość zamiast wyjątku
- Do każdej strategii dodaj testy jednostkowe w tests/Warehouse.UnitTests/Inventory/ (min. 4 przypadki każda)

## Poza zakresem

- IStockPostingService (T16)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T15.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~14k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T15b`).
