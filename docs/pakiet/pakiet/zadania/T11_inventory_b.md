# T11 — Inventory B — Reservation, CostLayer, SkuCostBalance

**Fala:** 5 · **Zależy od:** T02, T03, T04, T05, T06 · **Szacunek wyjścia:** ~8k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_4_inventory.md` (tylko Reservation, ReservationLine, ReservationAllocation, CostLayer, SkuCostBalance)
- `domena/D10_maszyny_stanow.md` (maszyna rezerwacji)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md`, `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T06.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T11 — Inventory B — Reservation, CostLayer, SkuCostBalance.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Inventory/: Reservation, ReservationLine, ReservationAllocation, CostLayer, SkuCostBalance

## Zakres i reguły szczegółowe

- Reservation: Create, AddLine, Allocate (przyjmuje już gotowy wynik alokacji jako parametr — sama strategia to T15), Release, Expire(now), Fulfill, Cancel; przejścia wg maszyny stanów
- CostLayer.Consume(qty) → (consumed, cost) i SkuCostBalance.AddReceipt/ConsumeAtAverage
- Zdarzenia z T06

## Poza zakresem

- Strategie alokacji i wyceny (T15)
- Zmiany w StockLevel

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T11.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~8k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T11b`).
