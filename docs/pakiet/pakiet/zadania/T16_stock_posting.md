# T16 — IStockPostingService — księgowanie ruchów

**Fala:** 7 · **Zależy od:** T10, T12, T15 · **Szacunek wyjścia:** ~12k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D07_serwisy_domenowe.md` (IStockPostingService)
- `domena/D11_niezmienniki.md`
- RAPORT_T10, RAPORT_T12, RAPORT_T15
- **Raporty** zależności: `RAPORT_T10.md`, `RAPORT_T12.md`, `RAPORT_T15.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T16 — IStockPostingService — księgowanie ruchów.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Inventory/Posting/: IStockPostingService, StockPostingService, PostingResult, PostingContext
- tests/Warehouse.UnitTests/Inventory/StockPostingServiceTests.cs

## Zakres i reguły szczegółowe

- Post(documentId, drafts, affectedLevels, context): dla każdego MovementDraft tworzy StockMovement (Sequence nadawany przez `context`/delegat — `Func<long> nextSequence` w PostingContext), aktualizuje StockLevel.Apply, liczy koszt przez IValuationStrategy i aktualizuje SkuCostBalance/CostLayer (przekazane w kontekście)
- Kolejność: najpierw ruchy ujemne, potem dodatnie w ramach jednego dokumentu (MM Immediate)
- Zwraca PostingResult: StockMovement[], zmienione StockLevel[], nowe CostLayer[], zdarzenia StockMoved/NegativeStockDetected
- Testy: PZ, WZ z FEFO, MM Immediate, odmowa przy braku stanu, idempotencja po Sequence

## Poza zakresem

- Dostęp do bazy
- Slice ApproveDocument

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T16.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~12k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T16b`).
