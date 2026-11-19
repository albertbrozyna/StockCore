# ARCHITECTURE — StockCore / Warehouse

Skrót; szczegóły: `ZALOZENIA.md` §4–§7, `pakiet/pakiet/01_KONWENCJE_PROJEKTU.md` (kontrakt K-1…K-10) i `pakiet/pakiet/domena/*.md`.

## Styl
Cztery projekty (.NET 10 / C# 14) — **vertical slice + DDD**. Logika w agregatach; handlery orkiestrują. Testy w osobnych projektach.

```text
src/src/
├── Warehouse.Domain/          # SharedKernel + konteksty (Tenancy, Catalog, Locations, Inventory, Documents, Fulfillment, Stocktaking, Alerts); bez zależności
├── Warehouse.Application/     # → Domain. Abstractions (IClock, ICurrentUser, IUnitOfWork, CQRS), Behaviors, DI
├── Warehouse.Infrastructure/  # → Application. Persistence (DbContext, Conventions, Interceptors, Configurations, Migrations), Json, SystemClock
└── Warehouse.Api/             # → Infrastructure. Web API, Program.cs, Features/ (slice = endpoint + command + handler + validator)
src/tests/{UnitTests,IntegrationTests,ArchitectureTests}
```

Kierunek zależności wymusza struktura projektów (Domain nie referuje niczego; Application nie widzi EF ani ASP.NET).

## Kluczowe decyzje (skrót K-1…K-10)
- Wszystkie ID, value objecty i enumy w `Warehouse.Domain/SharedKernel/…`; konteksty mają tylko encje, zdarzenia, serwisy.
- Domena bez EF/ASP.NET i bez NuGet (poza BCL); fabryki `Create` rzucają `DomainException`; `Result<T>` w Application.
- EF Core + Npgsql, TPH dla dokumentów, multi-tenancy: shared schema + `TenantId` + globalny query filter.
- Własny dispatcher CQRS (bez MediatR); księgowanie ruchów synchronicznie w jednej transakcji z zatwierdzeniem dokumentu.
- Odwołania między agregatami wyłącznie przez ID; slice'y nie wołają innych slice'ów.

## Zasady kodu
`#nullable enable`, file-scoped namespaces, jeden typ publiczny na plik, klasy `sealed`, brak `DateTime.Now` (używaj `IClock`), `TreatWarningsAsErrors`, centralne wersje pakietów (`Directory.Packages.props`).

## Stan wdrożenia
Zob. [`ROADMAP.md`](ROADMAP.md). Raporty zadań: [`raporty/`](raporty/).
