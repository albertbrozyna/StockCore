# T10 — Inventory A — StockMovement, StockLevel, Lot, SerialItem

**Fala:** 5 · **Zależy od:** T02, T03, T04, T05, T06 · **Szacunek wyjścia:** ~12k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_4_inventory.md` (tylko StockMovement, StockLevel, Lot, SerialItem)
- `domena/D10_maszyny_stanow.md` (SerialStatus: patrz niezmiennik)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md`, `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T06.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T10 — Inventory A — StockMovement, StockLevel, Lot, SerialItem.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Inventory/: StockMovement (ImmutableTenantEntity), StockLevel, Lot, SerialItem
- Domain/Inventory/Posting/: MovementDraft, StockLevelKey (record struct: LocationId, SkuId, LotId?, StockStatus)

## Zakres i reguły szczegółowe

- StockLevel.Apply(StockMovement, allowNegative): idempotencja po Sequence, niezmienniki OnHand/Reserved; Reserve/ReleaseReservation/CommitReservation; `Available` jako computed
- StockMovement: fabryka `Create(...)` waliduje znak QuantityDelta względem MovementType.Direction i ReversesMovementId dla Reversal
- Lot: Block/Release/MoveToQuarantine/IsExpired/RemainingShelfLifeDays
- SerialItem: tabela dozwolonych przejść statusów + metody Receive/MoveTo/Reserve/Release/Issue/Return/Scrap
- MovementDraft: pola potrzebne do zbudowania StockMovement (Type, WarehouseId, LocationId, SkuId, LotId?, SerialItemId?, StockStatus, QuantityDelta, UnitCost?, DocumentLineId, TransferPairId?, ReversesMovementId?)

## Poza zakresem

- Reservation, CostLayer, SkuCostBalance (T11)
- IStockPostingService (T16)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T10.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~12k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T10b`).
