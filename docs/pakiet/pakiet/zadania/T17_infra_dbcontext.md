# T17 — Infrastruktura — DbContext, konwencje, interceptory

**Fala:** 4 · **Zależy od:** T01, T02, T07 · **Szacunek wyjścia:** ~12k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D12_persystencja.md`
- `domena/D01_hierarchia.md` (§1.3)
- `domena/D02_sharedkernel.md`
- RAPORT_T01, RAPORT_T02, RAPORT_T07
- **Raporty** zależności: `RAPORT_T01.md`, `RAPORT_T02.md`, `RAPORT_T07.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T17 — Infrastruktura — DbContext, konwencje, interceptory.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Infrastructure/Persistence/WarehouseDbContext.cs (szkielet bez DbSetów kontekstów — DbSety dopisują T18–T20; zastosuj `ApplyConfigurationsFromAssembly`)
- Conventions/: konwencje decimal(18,4), strongly-typed ID, enum→string, nazwy schematów z folderu konfiguracji
- Interceptors/: AuditingInterceptor, TenantSaveChangesGuard, SoftDeleteInterceptor, ImmutableEntityGuard, DomainEventsInterceptor, OutboxInterceptor
- Filters: rozszerzenie `ApplyTenantAndSoftDeleteFilters(modelBuilder, ITenantContext)` dla IHasTenant/ISoftDeletable
- Infrastructure/Persistence/Outbox/: OutboxMessage + konfiguracja + OutboxProcessor (BackgroundService, szkielet publikujący przez IIntegrationEventPublisher)
- Infrastructure/DependencyInjection.cs — AddPersistence(connectionString)

## Zakres i reguły szczegółowe

- Wersjonowanie: `Version` → xmin (`IsRowVersion`) w konwencji dla IVersioned
- ImmutableEntityGuard rzuca wyjątek przy Modified/Deleted na ImmutableTenantEntity
- TenantSaveChangesGuard: zapis encji z innym TenantId niż kontekst → wyjątek
- Testy interceptorów w tests/Warehouse.IntegrationTests (Testcontainers Postgres) — min. po jednym teście na interceptor, o ile masz Dockera; inaczej opisz scenariusze w raporcie

## Poza zakresem

- Konfiguracje konkretnych encji (T18–T20)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T17.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~12k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T17b`).
