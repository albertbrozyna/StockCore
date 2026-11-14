# T09 — Locations (agregaty)

**Fala:** 5 · **Zależy od:** T02, T04, T05, T06 · **Szacunek wyjścia:** ~8k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D06_0_format.md`
- `domena/D06_3_locations.md`
- **Raporty** zależności: `RAPORT_T02.md`, `RAPORT_T04.md`, `RAPORT_T05.md`, `RAPORT_T06.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T09 — Locations (agregaty).
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/Locations/: Warehouse, Location
- Domain/Locations/Services/: ILocationCapacityPolicy + implementacja `LocationCapacityPolicy`

## Zakres i reguły szczegółowe

- Location: drzewo (ParentId, Path, Level), Move() z wykrywaniem cykli (przyjmij `IReadOnlyCollection<Location>` przodków jako parametr), Block/Unblock
- Warehouse: SetDefaultLocations, Deactivate (parametr `bool hasStock` dostarczony z zewnątrz)
- Zdarzenia z T06

## Poza zakresem

- EF (T18)
- Dostęp do bazy w domenie

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T09.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~8k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T09b`).
