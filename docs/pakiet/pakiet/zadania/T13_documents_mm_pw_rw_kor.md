# T13 — Documents — MM, PW, RW, KOR

**Fala:** 5 · **Zależy od:** T12 · **Szacunek wyjścia:** ~10k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_5_documents.md` (TransferDocument, InternalReceiptDocument, InternalIssueDocument, CorrectionDocument)
- RAPORT_T12 (sygnatury WarehouseDocument/DocumentLine — załącz!)
- **Raporty** zależności: `RAPORT_T12.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T13 — Documents — MM, PW, RW, KOR.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Documents/: TransferDocument + TransferLine, InternalReceiptDocument + InternalReceiptLine, InternalIssueDocument + InternalIssueLine, CorrectionDocument + CorrectionLine

## Zakres i reguły szczegółowe

- TransferDocument: Immediate → TransferOut+TransferIn (wspólny TransferPairId); TwoStep → TransferOut, potem `ReceiveTransfer(...)` → TransferIn; status InTransit/Completed
- CorrectionDocument: nigdy nie modyfikuje oryginału; ruchy CorrectionIncrease/Decrease
- InternalReceipt/Issue: Reason (w tym StocktakeAdjustment)
- Każda klasa implementuje BuildMovements() i ValidateForApproval()

## Poza zakresem

- Księgowanie (T16)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T13.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~10k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T13b`).
