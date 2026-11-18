# ARCHITECTURE — StockCore / Warehouse

Skrót; szczegóły: `ZALOZENIA.md` §4–§7, `pakiet/pakiet/01_KONWENCJE_PROJEKTU.md` (kontrakt K-1…K-10) i `pakiet/pakiet/domena/*.md`.

## Styl
Jeden projekt Web API (`src/src/Warehouse.Api`, .NET 10 / C# 14) — **vertical slice + DDD**. Logika w agregatach; handlery orkiestrują. Testy w osobnych projektach.

```text
src/src/Warehouse.Api/
├── Domain/{SharedKernel,Tenancy,Catalog,Locations,Inventory,Documents,Fulfillment,Stocktaking,Alerts}
├── Application/Abstractions/   # IClock, ICurrentUser, ITenantContext, IUnitOfWork, CQRS
├── Features/                   # slice = use case (endpoint + command + handler + validator)
├── Infrastructure/Persistence/ # DbContext, Conventions, Interceptors, Configurations, Migrations
└── Shared/                     # cross-cutting (np. Json)
src/tests/{UnitTests,IntegrationTests,ArchitectureTests}
```

## Kluczowe decyzje (skrót K-1…K-10)
- Wszystkie ID, value objecty i enumy w `Domain/SharedKernel/…`; konteksty mają tylko encje, zdarzenia, serwisy.
- Domena bez EF/ASP.NET i bez NuGet (poza BCL); fabryki `Create` rzucają `DomainException`; `Result<T>` w Application.
- EF Core + Npgsql, TPH dla dokumentów, multi-tenancy: shared schema + `TenantId` + globalny query filter.
- Własny dispatcher CQRS (bez MediatR); księgowanie ruchów synchronicznie w jednej transakcji z zatwierdzeniem dokumentu.
- Odwołania między agregatami wyłącznie przez ID; slice'y nie wołają innych slice'ów.

## Zasady kodu
`#nullable enable`, file-scoped namespaces, jeden typ publiczny na plik, klasy `sealed`, brak `DateTime.Now` (używaj `IClock`), `TreatWarningsAsErrors`, centralne wersje pakietów (`Directory.Packages.props`).

## Stan wdrożenia
Zob. [`ROADMAP.md`](ROADMAP.md). Raporty zadań: [`raporty/`](raporty/).
