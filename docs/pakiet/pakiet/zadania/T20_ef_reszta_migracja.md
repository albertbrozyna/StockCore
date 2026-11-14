# T20 — EF — konfiguracje: Fulfillment, Stocktaking, Alerts, Platforma + migracja początkowa

**Fala:** 8 · **Zależy od:** T14, T17, T18, T19 · **Szacunek wyjścia:** ~8k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D12_persystencja.md`
- `domena/D06_6_fulfillment.md`
- `domena/D06_7_stocktaking.md`
- `domena/D06_8_alerts.md`
- `domena/D06_9_platforma.md`
- RAPORT_T14, RAPORT_T17, RAPORT_T18, RAPORT_T19
- **Raporty** zależności: `RAPORT_T14.md`, `RAPORT_T17.md`, `RAPORT_T18.md`, `RAPORT_T19.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T20 — EF — konfiguracje: Fulfillment, Stocktaking, Alerts, Platforma + migracja początkowa.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Configurations/Fulfillment|Stocktaking|Alerts|Platform/*Configuration.cs (w tym IdempotencyRecord, AuditLogEntry, OutboxMessage jeśli nie zrobione w T17)
- Migrations/<timestamp>_Initial.cs (dotnet ef migrations add Initial) + SQL: trigger niemutowalności StockMovement z T19
- docs/architecture/database.md — schemat tabel w Mermaid ER

## Zakres i reguły szczegółowe

- Pełna migracja początkowa obejmująca wszystkie schematy
- Weryfikacja: `dotnet ef database update` na Postgres z docker-compose; test integracyjny „model się buduje i migracja przechodzi"

## Poza zakresem

- Seedy

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T20.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~8k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T20b`).
