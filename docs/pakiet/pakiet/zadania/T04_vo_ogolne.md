# T04 — Value objecty ogólne

**Fala:** 3 · **Zależy od:** T02, T03 · **Szacunek wyjścia:** ~7k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D04_value_objects.md`
- `domena/D02_sharedkernel.md` (ValueObject, Guard)
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T03.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T04 — Value objecty ogólne.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/SharedKernel/ValueObjects/: Quantity, UnitCode, Money, CurrencyCode, Dimensions, Weight, DateRange, Address, ContactInfo, TaxId

## Zakres i reguły szczegółowe

- Quantity: Add, Subtract, Negate, Multiply(decimal), IsZero, IsPositive; różne jednostki → DomainException `Common.UnitMismatch`
- Money: Add/Subtract tylko ta sama waluta; Multiply; Zero(currency)
- UnitCode: UPPER-trim, 1–10 znaków
- CurrencyCode: ISO 4217, 3 litery, walidacja po wbudowanej liście najpopularniejszych (min. PLN, EUR, USD, GBP, CZK, CHF)
- Dimensions: Volume (computed); Weight: ToKilograms()
- TaxId: walidacja NIP (suma kontrolna) dla `PL`, inne kraje tylko niepusty numer

## Poza zakresem

- Value objecty domenowe (T05)
- Konwertery EF

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T04.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~7k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T04b`).
