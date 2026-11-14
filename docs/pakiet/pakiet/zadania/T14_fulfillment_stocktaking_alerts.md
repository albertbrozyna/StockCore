# T14 — Fulfillment + Stocktaking + Alerts

**Fala:** 5 · **Zależy od:** T02, T03, T04, T05, T06 · **Szacunek wyjścia:** ~10k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_6_fulfillment.md`
- `domena/D06_7_stocktaking.md`
- `domena/D06_8_alerts.md`
- `domena/D10_maszyny_stanow.md` (picking, stocktake)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md`, `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T06.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T14 — Fulfillment + Stocktaking + Alerts.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Fulfillment/: PickingOrder, PickingLine, PutawayTask
- Domain/Stocktaking/: StockTake, StockTakeLine
- Domain/Alerts/: AlertRule, Alert

## Zakres i reguły szczegółowe

- PickingOrder: przejścia wg maszyny stanów, ConfirmPick, ReportShort
- PutawayTask.Complete(actualLocationId)
- StockTake.Start przyjmuje snapshot (lista `StockLevelSnapshot` jako parametr) i tworzy linie; Approve zwraca opis korekt (`StockTakeAdjustment[]`: kierunek, SkuId, LocationId, LotId?, ilość) — samo tworzenie dokumentów PW/RW robi slice
- Alert.Acknowledge/Resolve; AlertRule — dane + walidacja
- Zdarzenia z T06

## Poza zakresem

- Strategie putaway (T15)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T14.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~10k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T14b`).
