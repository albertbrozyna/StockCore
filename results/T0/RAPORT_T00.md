# RAPORT_T00 — Szkielet rozwiązania
Status: DONE
Zbudowano: nie (brak `dotnet` w środowisku; kod przejrzany pod kątem kompilacji, csproj/props zwalidowane jako XML) · Testy: nie dotyczy (nieuruchomione)

## Pliki
- Warehouse.sln — ręcznie napisany klasyczny .sln (4 projekty, foldery `src`/`tests`)
- global.json — SDK 10.0.100, `rollForward: latestFeature`
- Directory.Build.props — net10.0, C# 14, nullable, ImplicitUsings, TreatWarningsAsErrors, InvariantGlobalization
- Directory.Packages.props — Central Package Management, wszystkie wersje pakietów
- .editorconfig — file-scoped namespaces (warning), styl, LF
- .gitignore, .dockerignore — patrz „Odstępstwa"
- src/Warehouse.Api/Warehouse.Api.csproj — Web SDK; Npgsql EF, EF Design, FluentValidation, Serilog.AspNetCore, OpenTelemetry (tylko referencje)
- src/Warehouse.Api/Program.cs — minimalny host, `GET /health`, `public partial class Program`
- src/Warehouse.Api/appsettings.json — minimalna konfiguracja logowania
- src/Warehouse.Api/** (.gitkeep) — pełne drzewo folderów z KONWENCJE §1 (+ `Domain/Inventory/Posting/` z K-10, `Configurations/<Kontekst>/` dla 8 kontekstów)
- tests/Warehouse.UnitTests, tests/Warehouse.ArchitectureTests — csproj (xUnit, Shouldly; Architecture + NetArchTest.Rules) + referencja do Api
- tests/Warehouse.IntegrationTests — csproj (+ Mvc.Testing, Testcontainers.PostgreSql) + `HealthEndpointTests.cs`
- Dockerfile — multi-stage (sdk:10.0 → aspnet:10.0-noble-chiseled), port 8080, użytkownik nie-root
- docker-compose.yml — PostgreSQL 17 (healthcheck) + usługa `api`
- .github/workflows/ci.yml — restore, build, test na ubuntu-latest
- README.md, docs/{00-overview.md, adr, domain, api, architecture, runbooks}

## Publiczne API (tylko sygnatury — to jest kontrakt dla następnych zadań)
```csharp
// global namespace (top-level statements) — src/Warehouse.Api/Program.cs
public partial class Program;

// namespace Warehouse.IntegrationTests
public sealed class HealthEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{ public Task Health_returns_200(); }
```
Endpoint: `GET /health` → 200 `{ "status": "Healthy" }`.

## Odstępstwa i uwagi
- **Brak `dotnet`** → nie zbudowano. Wersje pakietów w `Directory.Packages.props` wpisane z pamięci, bez weryfikacji w NuGet — przy pierwszym `dotnet restore` mogą wymagać korekty (szczególnie 10.0.0 dla EF/Npgsql/Mvc.Testing, FluentValidation 12.0.0, OTel 1.12.0).
- `Warehouse.sln` napisany ręcznie jako klasyczny format (SDK 10 domyślnie tworzy `.slnx`); GUID-y projektów są umowne. Można zastąpić przez `dotnet new sln` + `dotnet sln add`.
- Dodano pliki spoza listy: `.gitignore`, `.dockerignore`, `src/Warehouse.Api/appsettings.json`, `docs/00-overview.md`, `HealthEndpointTests.cs` (smoke test, żeby CI miało co uruchomić; nie jest kodem domenowym).
- `public partial class Program;` jest w tym samym pliku co top-level statements (wymóg `WebApplicationFactory`); formalnie drugi typ publiczny w pliku, ale to znacznik generowanej klasy.
- Wybrano **Shouldly** (zamiast FluentAssertions — FA ma komercyjną licencję od v8).
- `TreatWarningsAsErrors` + `AnalysisLevel=latest-recommended` może ujawnić ostrzeżenia analizatorów przy pierwszym buildzie.
- Serilog/OpenTelemetry/FluentValidation: tylko referencje, bez konfiguracji w `Program.cs` (zgodnie z zakresem).
- Projekty UnitTests i ArchitectureTests nie zawierają jeszcze testów (`dotnet test` przejdzie z ostrzeżeniem o braku testów).
- Docker: `.dockerignore` wyklucza `tests/` i `docs/`; obraz kopiuje tylko `src/` + pliki props/global.json.
- Pytanie do architekta: czy NetArchTest (zgodnie z K-4/§5.4) ma zostać jedynym narzędziem, czy dodać ArchUnitNET (ZALOZENIA §4.4 wspomina oba)?

## Niewykonane (TODO dla kontynuacji)
- Weryfikacja `dotnet build` / `dotnet test` i korekta wersji pakietów.
- Testy architektury (T-późniejsze, gdy powstanie kod domeny).
- `launchSettings.json` (nie wymagany).

## Wskazówki dla zależnych zadań
- Folder `Domain/SharedKernel/{Abstractions,Ids,ValueObjects,Enums}` i `Application/Abstractions` są gotowe (z `.gitkeep` — usuń `.gitkeep` przy dodawaniu plików, nie jest to wymagane).
- Pakiety są centralne: nowy `PackageReference` w csproj bez `Version`, wersja w `Directory.Packages.props`.
- Warning-as-error: brakujące `using`/nullable naruszenia zepsują build — nie wyłączaj globalnie.
- Domena bez NuGet (K-4): EF/ASP.NET dostępne w projekcie, ale tylko dla Infrastructure/Features; pilnują tego przyszłe testy architektury.
