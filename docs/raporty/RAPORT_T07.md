# RAPORT_T07 — Abstrakcje aplikacyjne (Application/Abstractions)
Status: DONE
Zbudowano: tak (0 warnings) · Testy: przeszły (6 nowych w `DispatcherPipelineTests`; 54 jednostkowe łącznie)

## Pliki
- `Application/Abstractions/` — `IClock`, `ICurrentUser`, `ITenantContext`, `IUnitOfWork`, `IDomainEventDispatcher`, `IIntegrationEventPublisher`, `IIdempotencyService`, `IdempotencyResult`, `IdempotencyStatus`, `IAuditLogWriter`, `AuditLogEntry`.
- `Application/Abstractions/Messaging/` — `ICommand<T>`, `IQuery<T>`, `ICommandHandler`, `IQueryHandler`, `IPipelineBehavior`, `IDispatcher`, `Dispatcher`; internal: `RequestInvoker<TResult>`, `CommandInvoker`, `QueryInvoker`.
- `Application/Behaviors/` — `ValidationBehavior`, `TenantGuardBehavior`, `TransactionBehavior`, `LoggingBehavior`.
- `Application/DependencyInjection.cs` — `AddApplication()`, `AddHandlersFromAssembly(Assembly)`.
- `tests/Warehouse.UnitTests/Application/DispatcherPipelineTests.cs`.

## Publiczne API
```csharp
namespace Warehouse.Api.Application.Abstractions;
public interface IClock { DateTimeOffset UtcNow { get; } DateOnly Today(string timeZone); }
public interface ICurrentUser { UserId? UserId { get; } string? Name { get; } IReadOnlySet<string> Roles { get; } bool HasPermission(string permission); }
public interface ITenantContext { TenantId TenantId { get; } bool IsResolved { get; } }
public interface IUnitOfWork { Task<int> SaveChangesAsync(CancellationToken); Task BeginTransactionAsync(CancellationToken); Task CommitAsync(CancellationToken); Task RollbackAsync(CancellationToken); }
public interface IDomainEventDispatcher { Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct); }
public interface IIntegrationEventPublisher { Task PublishAsync<T>(T @event, CancellationToken ct) where T : notnull; }
public enum IdempotencyStatus { Started, Completed, InProgress, HashMismatch }
public sealed record IdempotencyResult(IdempotencyStatus Status, string? StoredResponse = null);
public interface IIdempotencyService { Task<IdempotencyResult> BeginAsync(string key, string requestHash, CancellationToken); Task CompleteAsync(string key, string response, CancellationToken); }
public sealed record AuditLogEntry(TenantId TenantId, UserId? UserId, DateTimeOffset OccurredAt, string Action, string ObjectType, string? ObjectId, string? Details = null);
public interface IAuditLogWriter { Task WriteAsync(AuditLogEntry entry, CancellationToken ct); }

namespace Warehouse.Api.Application.Abstractions.Messaging;
public interface ICommand<TResult>; public interface IQuery<TResult>;
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> { Task<Result<TResult>> HandleAsync(TCommand, CancellationToken); }
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult> { Task<Result<TResult>> HandleAsync(TQuery, CancellationToken); }
public interface IPipelineBehavior<in TRequest, TResult> { Task<Result<TResult>> HandleAsync(TRequest, Func<Task<Result<TResult>>> next, CancellationToken); }
public interface IDispatcher { Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult>, CancellationToken); Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult>, CancellationToken); }
public sealed class Dispatcher(IServiceProvider services) : IDispatcher;

namespace Warehouse.Api.Application.Behaviors;
public sealed class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>;                       // Validation.Failed
public sealed class TenantGuardBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>;                      // Tenant.NotResolved (Forbidden)
public sealed class TransactionBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : ICommand<TResult>;
public sealed partial class LoggingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>;

namespace Warehouse.Api.Application;
public static class DependencyInjection { IServiceCollection AddApplication(this IServiceCollection); IServiceCollection AddHandlersFromAssembly(this IServiceCollection, Assembly); }
```

## Działanie
- `Dispatcher` rozwiązuje handler z DI (scoped), buduje pipeline z zarejestrowanych `IPipelineBehavior<,>` (pierwszy zarejestrowany = najbardziej zewnętrzny) i cache'uje typy-invokery.
- Kolejność z `AddApplication()`: Logging → Validation → TenantGuard → Transaction → handler. `TransactionBehavior` ma ograniczenie `ICommand<>`, więc DI pomija go dla zapytań.
- `TransactionBehavior`: Begin → handler → sukces: `SaveChanges` + `Commit`; błąd `Result` lub wyjątek: `Rollback` (wyjątek jest rzucany dalej). Handler **nie** woła `SaveChanges` sam (K-6: jedna transakcja).
- Walidatory (`IValidator<T>`) i handlery rejestrowane skanowaniem assembly; `AddHandlersFromAssembly` jest publiczne, żeby testy/inne assembly mogły dodać swoje.

## Odstępstwa i uwagi
- `IUnitOfWork` rozszerzono o `RollbackAsync` (D08 ma „…” przy transakcjach; `TransactionBehavior` go potrzebuje) i `ct` w `BeginTransactionAsync`.
- `IdempotencyResult` / `IdempotencyStatus` / `AuditLogEntry` — D08 nie definiuje pól; przyjęto minimalne wyżej (**do potwierdzenia przez architekta**). `CompleteAsync(key, response, ct)` — parametry dobrane przeze mnie.
- Behaviors `Idempotency` i `Auditing` z D08 są poza listą plików T07 — nie zrobione (interfejsy gotowe).
- `TenantGuardBehavior` odrzuca **każde** żądanie bez tenanta; endpointy anonimowe (logowanie, rejestracja tenanta) będą potrzebowały znacznika wyłączenia — decyzja przy pierwszym takim slice'ie.
- `IIntegrationEventPublisher.PublishAsync<T>` dostał ograniczenie `where T : notnull` (analizator).
- `ICommand<>`/`IQuery<>` to interfejsy znacznikowe — `#pragma warning disable CA1040` w tych dwóch plikach (wyjątek wynika z kontraktu CQRS).
- `AddApplication()` nie jest jeszcze wywołane w `Program.cs` (poza zakresem; wymaga implementacji `ITenantContext`/`IUnitOfWork`).
- Brak implementacji `IClock` (D08 dopuszcza `TimeProvider`) — do T17/API.
- `FluentValidation.DependencyInjectionExtensions` nie jest w pakietach — walidatory skanowane własnym kodem.

## Niewykonane (TODO dla kontynuacji)
- Behaviors Idempotency/Auditing; wywołanie `AddApplication()` w `Program.cs`.

## Wskazówki dla zależnych zadań
- T17: implementacja `IUnitOfWork` (DbContext) i `IDomainEventDispatcher`; behaviors zakładają `Scoped`.
- Slice'y: polecenia/zapytania jako `sealed record : ICommand<T>`; handler zwraca `Result<T>`; wyjątki `DomainException` z domeny trzeba mapować na `Result` w handlerze lub middleware API.
