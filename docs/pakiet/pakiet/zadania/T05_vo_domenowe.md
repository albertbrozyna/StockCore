# T05 — Value objecty domenowe

**Fala:** 4 · **Zależy od:** T02, T03, T04 · **Szacunek wyjścia:** ~7k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D04_value_objects.md`
- `domena/D05_enumeracje.md` (tylko BarcodeType)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md`, `RAPORT_T04.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T05 — Value objecty domenowe.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/SharedKernel/ValueObjects/: SkuCode, Barcode, LotNumber, SerialNumber, LocationCode, WarehouseCode, DocumentNumber, ExternalReference, ShelfLife, LocationCapacity, TenantCode, TenantSettings, DocumentNumberingPolicy

## Zakres i reguły szczegółowe

- Barcode: walidacja sumy kontrolnej EAN-13/EAN-8/UPC-A (algorytm modulo 10), pozostałe typy bez sumy
- TenantCode: regex `[a-z0-9-]{3,40}`
- DocumentNumber: Prefix, Year, Sequence, Full; fabryka `Create(prefix, year, sequence, padding)` → np. `PZ/WH1/2026/000123`
- TenantSettings i DocumentNumberingPolicy jako `sealed record` z sensownymi wartościami domyślnymi (`Default`)
- SkuCode: UPPER-trim ≤ 50; LotNumber ≤ 50; SerialNumber ≤ 100; LocationCode ≤ 40; WarehouseCode ≤ 10

## Poza zakresem

- Wartości z T04
- EF

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T05.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~7k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T05b`).
