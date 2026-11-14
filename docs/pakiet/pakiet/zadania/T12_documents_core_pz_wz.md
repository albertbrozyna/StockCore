# T12 — Documents core + PZ + WZ

**Fala:** 5 · **Zależy od:** T02, T03, T04, T05, T06, T10 · **Szacunek wyjścia:** ~12k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_5_documents.md` (WarehouseDocument, DocumentLine, ReceiptDocument, IssueDocument)
- `domena/D10_maszyny_stanow.md` (maszyna dokumentu)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md`, `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T06.md`, `RAPORT_T10.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T12 — Documents core + PZ + WZ.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Documents/: WarehouseDocument (abstract), DocumentLine (abstract), ReceiptDocument + ReceiptLine, IssueDocument + IssueLine

## Zakres i reguły szczegółowe

- WarehouseDocument: AddLine/UpdateLine/RemoveLine (tylko Draft), Approve(DocumentNumber, now), Cancel, MarkCorrected; `protected abstract ValidateForApproval()`, `public abstract IReadOnlyList<MovementDraft> BuildMovements()`
- BaseQuantity linii przekazywane z zewnątrz przy AddLine (konwersja robi slice przez IUnitConverter) — dokument jej nie liczy
- Reguły ReceiptLine (Lot/Serial/ExpiryDate) i IssueLine zgodnie z D06_5 — walidacja w `ValidateForApproval()`, część wymagająca danych zewnętrznych (Sku.TrackingMode) przez parametr `IReadOnlyDictionary<SkuId, SkuTrackingInfo>` — zdefiniuj mały record `SkuTrackingInfo(TrackingMode, RequiresExpiryDate, ShelfLife?)` w Domain/Documents/
- Zdarzenia z T06

## Poza zakresem

- Pozostałe typy dokumentów (T13)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T12.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~12k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T12b`).
