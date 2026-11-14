# T21 — Testy jednostkowe: SharedKernel i Value Objecty

**Fala:** 9 · **Zależy od:** T04, T05, T02 · **Szacunek wyjścia:** ~9k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- RAPORT_T02, RAPORT_T04, RAPORT_T05
- `domena/D04_value_objects.md`
- **Raporty** zależności: `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T02.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T21 — Testy jednostkowe: SharedKernel i Value Objecty.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- tests/Warehouse.UnitTests/SharedKernel/*Tests.cs
- tests/Warehouse.UnitTests/ValueObjects/*Tests.cs

## Zakres i reguły szczegółowe

- Quantity/Money (operacje, niezgodne jednostki/waluty)
- Barcode (sumy kontrolne EAN-13, EAN-8, UPC-A), TaxId (NIP), TenantCode, DocumentNumber
- Entity equality, TenantEntity niezmienność TenantId, Enumeration.FromId/FromName, DocumentStatus.CanTransitionTo, MovementType znaki
- xUnit + Shouldly/FluentAssertions, nazewnictwo `Metoda_Warunek_Rezultat`

## Poza zakresem

- Testy encji domenowych (osobne zadania po zakończeniu agregatów)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T21.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~9k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T21b`).
