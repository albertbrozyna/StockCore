<!-- Fragment DOMENA.md (D08_abstrakcje_aplikacyjne). Pełny kontekst: ref/DOMENA.md -->

## 8. Abstrakcje aplikacyjne / infrastrukturalne (używane przez slice'y)

Nie należą do `Domain/`, ale są częścią kontraktu — implementator musi je założyć.

```csharp
public interface IClock                 { DateTimeOffset UtcNow { get; } DateOnly Today(string timeZone); }  // lub TimeProvider
public interface ICurrentUser           { UserId? UserId { get; } string? Name { get; } IReadOnlySet<string> Roles { get; } bool HasPermission(string permission); }
public interface ITenantContext         { TenantId TenantId { get; } bool IsResolved { get; } }               // z claimu JWT / nagłówka na APIM
public interface IUnitOfWork            { Task<int> SaveChangesAsync(CancellationToken ct); Task BeginTransactionAsync(...); Task CommitAsync(...); }
public interface IDomainEventDispatcher { Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct); }
public interface IIntegrationEventPublisher { Task PublishAsync<T>(T @event, CancellationToken ct); }          // zapis do outboxa
public interface IIdempotencyService    { Task<IdempotencyResult> BeginAsync(string key, string requestHash, CancellationToken ct); Task CompleteAsync(...); }
public interface IAuditLogWriter        { Task WriteAsync(AuditLogEntry entry, CancellationToken ct); }

// CQRS (własny lekki dispatcher — ADR-004)
public interface ICommand<TResult>      { }
public interface IQuery<TResult>        { }
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{ Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken ct); }
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{ Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken ct); }
public interface IPipelineBehavior<TRequest, TResult> { Task<Result<TResult>> HandleAsync(TRequest request, Func<Task<Result<TResult>>> next, CancellationToken ct); }
// behaviors: Validation, Tenant check, Idempotency, Transaction, Logging, Auditing
```

Interceptory EF Core (w `Infrastructure/Persistence/`): `AuditingInterceptor` (ustawia `Created*/Modified*`, `TenantId`), `TenantSaveChangesGuard` (blokuje zapis z innym `TenantId` niż kontekst), `SoftDeleteInterceptor`, `ImmutableEntityGuard`, `DomainEventsInterceptor` (zbiera zdarzenia po `SaveChanges`), `OutboxInterceptor` (zdarzenia integracyjne → `OutboxMessage` w tej samej transakcji).
