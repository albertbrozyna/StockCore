# T00 — Szkielet rozwiązania

**Fala:** 0 · **Zależy od:** brak · **Szacunek wyjścia:** ~8k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `ref/ZALOZENIA.md` (§4.3, §4.4, §5)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T00 — Szkielet rozwiązania.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Warehouse.sln
- Directory.Build.props, Directory.Packages.props (Central Package Management), .editorconfig, global.json (.NET 10)
- src/Warehouse.Api/Warehouse.Api.csproj + Program.cs (minimalny host, /health) + puste foldery z .gitkeep wg KONWENCJE §1
- tests/Warehouse.UnitTests, tests/Warehouse.IntegrationTests, tests/Warehouse.ArchitectureTests (csproj + referencje)
- Dockerfile (multi-stage), docker-compose.yml (PostgreSQL 17), .github/workflows/ci.yml (build + test)
- README.md, docs/ (szkielet: adr/, domain/, api/, architecture/, runbooks/)

## Zakres i reguły szczegółowe

- Pakiety: Npgsql.EntityFrameworkCore.PostgreSQL, FluentValidation, Serilog, OpenTelemetry (tylko referencje w Directory.Packages.props i csproj API)
- Pakiety testowe: xUnit, FluentAssertions lub Shouldly, Testcontainers.PostgreSql, NetArchTest.Rules

## Poza zakresem

- Jakikolwiek kod domenowy
- Konfiguracja Terraform / Azure

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T00.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~8k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T00b`).
