# Warehouse

System magazynowy (WMS-lite): .NET 10, vertical slice + DDD, PostgreSQL, multi-tenant.

## Szybki start

```bash
docker compose up -d postgres      # PostgreSQL 17
dotnet run --project src/Warehouse.Api
curl http://localhost:5000/health  # port wg launchSettings / ASPNETCORE_URLS
```

Pełne środowisko: `docker compose up --build` (API na `http://localhost:8080`).

## Struktura

- `src/Warehouse.Domain` → `Warehouse.Application` → `Warehouse.Infrastructure` → `Warehouse.Api` (Web API, Features)
- `tests/` — UnitTests, IntegrationTests (Testcontainers), ArchitectureTests (NetArchTest)
- `docs/` — dokumentacja (adr, domain, api, architecture, runbooks)

## Testy

```bash
dotnet test Warehouse.sln
```

Integracyjne testy z bazą wymagają działającego Dockera.
