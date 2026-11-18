# CLAUDE.md — StockCore

System magazynowy (WMS-lite), .NET 10, vertical slice + DDD, PostgreSQL. Dokumentacja po polsku; kod i identyfikatory po angielsku.

## Dokumentacja (czytaj przed pracą)
`docs/PRODUCT.md`, `docs/ARCHITECTURE.md`, `docs/ROADMAP.md`, `docs/PLAN_PRAC.md` (zadania T00–T22 i zależności), kontrakt: `docs/pakiet/pakiet/01_KONWENCJE_PROJEKTU.md` (wygrywa nad `domena/*.md`), brief zadania: `docs/pakiet/pakiet/zadania/Txx_*.md`.

## Komendy (katalog `src/`)
```bash
dotnet build Warehouse.sln
dotnet test Warehouse.sln --filter "FullyQualifiedName~UnitTests"   # integracyjne wymagają Dockera
```
Brak `dotnet` w środowisku cloud: `apt-get install -y dotnet-sdk-10.0`.

## Zasady pracy
1. Jedno zadanie = jeden commit konwencjonalny (`feat(scope): …`); osobne branche, nie `main`.
2. Nie wymyślaj funkcji; braki/sprzeczności → sekcja „Odstępstwa” w `docs/raporty/RAPORT_Txx.md` (szablon: `02_SZABLON_RAPORTU.md`).
3. Zadanie jest ukończone dopiero po zielonym build + testach; potem zaktualizuj `PLAN_PRAC.md`, `ROADMAP.md`, raport.
4. Nie poprawiaj kodu spoza zakresu zadania; nie usuwaj funkcji bez uzasadnienia.
5. Testuj moduł, którego dotyczy zmiana, nie cały zestaw.

## Kod
`#nullable enable`, file-scoped namespaces, jeden typ publiczny na plik, `sealed`, bez `DateTime.Now`, domena bez EF/ASP.NET/NuGet. `TreatWarningsAsErrors` — analizatory (CA*) łamią build; wyjątki tylko w `.editorconfig` z komentarzem. Wersje pakietów wyłącznie w `Directory.Packages.props` (w tym piny tranzytywne dla luk NuGet).
