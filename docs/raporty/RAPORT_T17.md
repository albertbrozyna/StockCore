# RAPORT_T17 — Infrastruktura — DbContext, konwencje, interceptory
Status: DONE (testy integracyjne z Postgresem — nie uruchomione, brak Dockera; zob. „Scenariusze”)
Zbudowano: tak (0 warnings) · Testy: przeszły (13 nowych w `PersistenceModelTests`, bez bazy; 84 jednostkowe łącznie)

## Pliki
- `Infrastructure/Persistence/WarehouseDbContext.cs` — szkielet + `DbSet<OutboxMessage>`; `ApplyConfigurationsFromAssembly`, schematy z folderów, konwencje domenowe, filtry.
- `Conventions/` — `PersistenceConventionsExtensions` (decimal(18,4), ID, enum→string, smart enum po `Name`), `EnumerationValueConverter<T>`, `ModelConventionsExtensions` (`ApplyConfigurationSchemas`, `ApplyDomainConventions`), `QueryFilterExtensions`, `ITenantFilterSource` (+ istniejące konwertery ID z T01).
- `Interceptors/` — `AuditingInterceptor`, `TenantSaveChangesGuard`, `SoftDeleteInterceptor`, `ImmutableEntityGuard`, `DomainEventsInterceptor`, `OutboxInterceptor`.
- `Outbox/` — `OutboxMessage`, `OutboxJson` (internal), `IOutboxMessageDispatcher`, `LoggingOutboxMessageDispatcher`, `OutboxIntegrationEventPublisher`, `OutboxProcessor`; `Configurations/Platform/OutboxMessageConfiguration.cs` (schemat `platform`, `jsonb`).
- `EfUnitOfWork`, `NoOpDomainEventDispatcher`, `Infrastructure/SystemClock.cs`, `Infrastructure/DependencyInjection.cs` (`AddPersistence`).
- `Shared/Json/EnumerationJsonConverter(+Factory)` — smart enum ↔ `Name` (payload outboxa).
- Domena (SharedKernel): wewnętrzne hooki `ICreationStamp`, `IModificationStamp`, `ITenantStamp`, `IDeletionStamp`, `IImmutableEntity`, implementowane jawnie przez klasy bazowe z T02 (interceptory nie znają generycznych typów).
- `tests/Warehouse.UnitTests/Infrastructure/PersistenceModelTests.cs`.

## Publiczne API
```csharp
public sealed class WarehouseDbContext(DbContextOptions<WarehouseDbContext>, ITenantContext) : DbContext, ITenantFilterSource
{ DbSet<OutboxMessage> OutboxMessages; TenantId CurrentTenantId; }   // DbSety kontekstów dopisują T18–T20
public interface ITenantFilterSource { TenantId CurrentTenantId { get; } }
public static class PersistenceConventionsExtensions { ModelConfigurationBuilder ConfigurePersistenceConventions(this ModelConfigurationBuilder); }
public static class ModelConventionsExtensions { ModelBuilder ApplyConfigurationSchemas(this ModelBuilder, Assembly); ModelBuilder ApplyDomainConventions(this ModelBuilder); }
public static class QueryFilterExtensions { ModelBuilder ApplyTenantAndSoftDeleteFilters<TContext>(this ModelBuilder, TContext context) where TContext : DbContext, ITenantFilterSource; }
public sealed class EnumerationValueConverter<T> : ValueConverter<T, string> where T : Enumeration;
public sealed class AuditingInterceptor(IClock, ICurrentUser, ITenantContext) : SaveChangesInterceptor;
public sealed class TenantSaveChangesGuard(ITenantContext) : SaveChangesInterceptor;
public sealed class SoftDeleteInterceptor(IClock, ICurrentUser) : SaveChangesInterceptor;
public sealed class ImmutableEntityGuard : SaveChangesInterceptor;
public sealed class OutboxInterceptor : SaveChangesInterceptor;
public sealed class DomainEventsInterceptor(IDomainEventDispatcher) : SaveChangesInterceptor;
public sealed class OutboxMessage { Guid Id; TenantId TenantId; string Type; string Payload; DateTimeOffset OccurredAt; DateTimeOffset? ProcessedAt; int Attempts; string? LastError; static Create(IIntegrationEvent); MarkProcessed(DateTimeOffset); MarkFailed(string) }
public interface IOutboxMessageDispatcher { Task DispatchAsync(OutboxMessage, CancellationToken); }
public sealed class OutboxIntegrationEventPublisher(WarehouseDbContext) : IIntegrationEventPublisher;
public sealed class OutboxProcessor : BackgroundService;      // poll 5 s, paczka 20, max 10 prób
public sealed class EfUnitOfWork(WarehouseDbContext) : IUnitOfWork;
public sealed class SystemClock : IClock;  public sealed class NoOpDomainEventDispatcher : IDomainEventDispatcher;
public static class DependencyInjection { IServiceCollection AddPersistence(this IServiceCollection, string connectionString); }   // Warehouse.Api.Infrastructure
```
Kody błędów: `Persistence.ImmutableEntity` (Conflict), `Tenancy.CrossTenantWrite` (Forbidden).
Kolejność interceptorów: ImmutableGuard → SoftDelete → Auditing → TenantGuard → Outbox → DomainEvents.

## Odstępstwa i uwagi
- **Filtr tenanta — zmiana sygnatury.** Zlecona `ApplyTenantAndSoftDeleteFilters(modelBuilder, ITenantContext)` jest błędna: EF cache'uje model, więc wartość z `ITenantContext` byłaby „wstrzyknięta” jako stała z pierwszego kontekstu i **wszystkie następne żądania widziałyby dane pierwszego tenanta** (potwierdzone testem na SQL). Rozwiązanie: filtr czyta `DbContext.CurrentTenantId` (`ITenantFilterSource`); nieustalony tenant → `default` → brak wyników (fail closed). Test `Query_filter_is_evaluated_per_context_tenant`.
- **`OutboxProcessor` a `IIntegrationEventPublisher`:** zlecenie każe publikować przez `IIntegrationEventPublisher`, ale D08 definiuje go jako zapis *do* outboxa — procesor czytający outbox i zapisujący do niego tworzyłby pętlę. `IIntegrationEventPublisher` jest więc zapisem do outboxa (`OutboxIntegrationEventPublisher`), a procesor wysyła przez nowy `IOutboxMessageDispatcher` (domyślnie tylko loguje; docelowo Service Bus). **Do potwierdzenia.**
- `OutboxMessage` nie jest `IHasTenant` (brak filtra), żeby procesor działał poza kontekstem tenanta; `TenantId` to zwykła kolumna.
- Dodane poza listą: `EfUnitOfWork`, `SystemClock`, `NoOpDomainEventDispatcher`, `ITenantFilterSource`, konwertery smart enum (EF + JSON) — potrzebne do działania `AddPersistence`/outboxa.
- `NoOpDomainEventDispatcher` odrzuca zdarzenia, dopóki nie powstaną handlery wewnętrzne (brak `IDomainEventHandler` w T07). Dispatch odbywa się po `SaveChanges`; handler zmieniający dane musi sam zapisać.
- `TenantSaveChangesGuard` blokuje też zapis encji tenantowych bez ustalonego tenanta (zadania w tle/seed muszą ustawić kontekst tenanta).
- Nazwy kolumn: PascalCase (brak pakietu snake_case; D12 podaje np. `on_hand` tylko poglądowo) — decyzja architekta. `Version` → `xmin` przez `IsRowVersion()` (zweryfikowane w modelu).
- `AddPersistence` nie jest wołane w `Program.cs` (brak implementacji `ITenantContext`/`ICurrentUser`).
- `OutboxProcessor` zakłada jedną instancję (brak `FOR UPDATE SKIP LOCKED`).
- Nullable enumy: konwerter rejestrowany osobno dla `T?`.

## Scenariusze testów integracyjnych (Testcontainers Postgres — do uruchomienia z Dockerem)
1. Auditing: zapis nowej encji → `CreatedAt/CreatedBy/TenantId` zapisane; aktualizacja → `ModifiedAt/By`.
2. TenantGuard: zapis encji z cudzym `TenantId` → `Tenancy.CrossTenantWrite`, transakcja bez zmian.
3. SoftDelete: `Remove` → wiersz zostaje z `IsDeleted=true`, `DeletedAt/By`; zapytanie bez `IgnoreQueryFilters` go nie zwraca.
4. ImmutableGuard: update/delete wpisu ledger → wyjątek; insert działa.
5. Outbox: zapis agregatu z zdarzeniem ✔ → w tej samej transakcji wiersz `platform.outbox_messages`; rollback usuwa oba; zdarzenie nie-integracyjne nie trafia do outboxa.
6. DomainEvents: dispatcher wywołany dopiero po udanym zapisie; po błędzie — wcale.
7. Filtr tenanta: dwa tenanty, ten sam typ → każdy widzi tylko swoje; brak tenanta → pusto.
8. `xmin`: równoległa edycja tej samej encji → `DbUpdateConcurrencyException`.
9. `OutboxProcessor.ProcessBatchAsync`: wiadomość oznaczona `ProcessedAt`; wyjątek dispatchera → `Attempts++`, `LastError`.

## Niewykonane (TODO dla kontynuacji)
- Testy Testcontainers (powyżej), `FOR UPDATE SKIP LOCKED` w procesorze, migracje (T20).

## Wskazówki dla zależnych zadań
- T18–T20: konfiguracje w `Configurations/<Kontekst>/` (schemat = nazwa folderu); dopisać `DbSet`-y; `Version`, `DomainEvents`, filtry i konwertery są już globalne (nie powtarzać). TPH dokumentów: filtr dotyczy korzenia hierarchii.
- Host/API: zarejestrować `ITenantContext`, `ICurrentUser`, wywołać `AddApplication()` i `AddPersistence(cs)`.
