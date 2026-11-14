# T18 — EF — konfiguracje: Tenancy, Catalog, Locations

**Fala:** 6 · **Zależy od:** T08, T09, T17 · **Szacunek wyjścia:** ~10k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D12_persystencja.md`
- `domena/D06_1_tenancy.md`
- `domena/D06_2_catalog.md`
- `domena/D06_3_locations.md`
- RAPORT_T08, RAPORT_T09, RAPORT_T17
- **Raporty** zależności: `RAPORT_T08.md`, `RAPORT_T09.md`, `RAPORT_T17.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T18 — EF — konfiguracje: Tenancy, Catalog, Locations.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Infrastructure/Persistence/Configurations/Tenancy|Catalog|Locations/*Configuration.cs (IEntityTypeConfiguration<T> na encję)
- Dopisanie DbSetów w WarehouseDbContext (tylko te konteksty)

## Zakres i reguły szczegółowe

- Schematy: tenancy, catalog, locations
- VO: `OwnsOne`/`HasConversion` (Quantity/Money jako 2 kolumny), kolekcje przez backing fields (`UsePropertyAccessMode(Field)`)
- Indeksy unikalne z D12 (zawsze z TenantId na początku)
- JSONB: Sku.Attributes
- Location: indeks po (TenantId, WarehouseId, Path)

## Poza zakresem

- Migracje (T20)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T18.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~10k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T18b`).
