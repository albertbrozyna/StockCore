# T03 — Enumeracje (zwykłe i smart enums)

**Fala:** 1 · **Zależy od:** brak · **Szacunek wyjścia:** ~5k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D05_enumeracje.md`
- `domena/D04_value_objects.md` (tylko by znać nazwy użyte jako typy)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T03 — Enumeracje (zwykłe i smart enums).
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/SharedKernel/Enums/*.cs — jeden plik na enum
- Smart enums: DocumentType, MovementType, DocumentStatus (dziedziczą po `Enumeration` — jeśli T02 nie jest gotowe, zaimplementuj je zakładając sygnaturę `abstract class Enumeration(int id, string name)` z D02 i zaznacz to w raporcie)

## Zakres i reguły szczegółowe

- Wszystkie enumy z §5.2 jako zwykłe `enum` (ContractorKind z `[Flags]`)
- DocumentType: Code, RequiresApproval, AffectsStock; instancje statyczne Receipt/Issue/Transfer/InternalReceipt/InternalIssue/Correction
- MovementType: Direction, AffectsValuation, Sign; wszystkie 11 instancji z tabeli §5.1
- DocumentStatus: Draft, Approved, InTransit, Completed, Cancelled + `CanTransitionTo(next)` wg D10 (dokument)

## Poza zakresem

- Logika poza metodami enumów

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T03.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~5k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T03b`).
