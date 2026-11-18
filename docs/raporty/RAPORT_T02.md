# RAPORT_T02 — SharedKernel — klasy bazowe i abstrakcje
Status: DONE
Zbudowano: tak (`dotnet build`, 0 warnings) · Testy: przeszły (11 nowych w `SharedKernelBaseTests`; 32 jednostkowe łącznie)

## Pliki
- `Domain/SharedKernel/Abstractions/` — `IEntity<TId>`, `IAggregateRoot`, `IDomainEvent`, `IHasDomainEvents`, `IHasTenant`, `ICreationAudited`, `IAuditable`, `ISoftDeletable`, `IVersioned` (zgodne z D02 §2.1).
- `Domain/SharedKernel/` — `Entity`, `AuditableEntity`, `TenantEntity`, `SoftDeletableTenantEntity`, `ImmutableTenantEntity`, `ValueObject`, `DomainEvent`, `DomainException`, `Error`, `ErrorType`, `Result`, `ResultOfT` (typ `Result<T>`), `Guard`. `Enumeration` pochodzi z T03 (bez zmian).
- `Warehouse.Api.csproj` — dodano `InternalsVisibleTo Warehouse.UnitTests` (testy metod `Stamp*`/`SetTenant`).
- `tests/Warehouse.UnitTests/SharedKernel/SharedKernelBaseTests.cs`.

## Publiczne API
```csharp
public abstract class Entity<TId> : IEntity<TId>, IHasDomainEvents, IEquatable<Entity<TId>> where TId : notnull
{ TId Id { get; protected init; } IReadOnlyCollection<IDomainEvent> DomainEvents; void ClearDomainEvents(); protected void Raise(IDomainEvent); ==, != ; protected Entity(); protected Entity(TId id); }
public abstract class AuditableEntity<TId> : Entity<TId>, IAuditable, IVersioned
{ DateTimeOffset CreatedAt; UserId? CreatedBy; DateTimeOffset? ModifiedAt; UserId? ModifiedBy; uint Version;   // wszystkie private set
  internal void StampCreated(DateTimeOffset, UserId?); internal void StampModified(DateTimeOffset, UserId?); }
public abstract class TenantEntity<TId> : AuditableEntity<TId>, IHasTenant { TenantId TenantId { get; private set; } internal void SetTenant(TenantId); }
public abstract class SoftDeletableTenantEntity<TId> : TenantEntity<TId>, ISoftDeletable
{ bool IsDeleted; DateTimeOffset? DeletedAt; UserId? DeletedBy; void Delete(); void Restore(); internal void StampDeleted(DateTimeOffset, UserId?); }
public abstract class ImmutableTenantEntity<TId> : Entity<TId>, IHasTenant, ICreationAudited
{ TenantId; CreatedAt; CreatedBy; internal void SetTenant(TenantId); internal void StampCreated(DateTimeOffset, UserId?); }
public abstract class ValueObject : IEquatable<ValueObject> { protected abstract IEnumerable<object?> GetEqualityComponents(); ==, != }
public abstract record DomainEvent(TenantId TenantId, DateTimeOffset OccurredAt) : IDomainEvent { Guid EventId { get; init; } }
public sealed class DomainException : Exception { DomainException(Error error); Error Error { get; } }
public sealed record Error(string Code, string Message, ErrorType Type) { static readonly Error None; }
public enum ErrorType { Validation, NotFound, Conflict, Forbidden, Unexpected }
public readonly struct Result { IsSuccess; IsFailure; Error; static Result Success(); static Result Failure(Error); static Result<T> Success<T>(T); static Result<T> Failure<T>(Error); }
public readonly struct Result<T> { IsSuccess; IsFailure; Error; T Value /* throws InvalidOperationException on failure */ }
public static class Guard { AgainstNull<T>(T?) ; AgainstNullOrWhiteSpace(string?); AgainstOutOfRange<T>(T,min,max); AgainstNegative<T>(T); AgainstDefault<TId>(TId) }
```

## Mechanizm stemplowania (dla T17)
Settery audytu i `TenantId` są `private set`; interceptor (ten sam assembly) używa metod `internal`: `StampCreated` (Added), `StampModified` (Modified), `SetTenant` (Added, z `ITenantContext`; agregat-rodzic może też wywołać je w fabryce dla encji zależnych), `StampDeleted` (gdy `IsDeleted` i `DeletedAt == null`). `SetTenant`: ten sam tenant = no-op, inny → `DomainException("SharedKernel.TenantImmutable")`; pusty ID → `Guard.Default`. Interceptor nie wymaga `InternalsVisibleTo`, bo leży w `Warehouse.Api`.

## Odstępstwa i uwagi
- `DomainEvent`: D02 mówi „wypełniane przez `IClock` przekazany z agregatu”, ale `IClock` leży w Application (T07), a domena nie może od niego zależeć. Konstruktor przyjmuje więc `(TenantId, DateTimeOffset occurredAt)`; agregat pobiera czas z `IClock`/parametru `now`.
- `Error.None` — dodany pomocniczo (wartość `Error` dla wyniku sukcesu; `default(Result)` jest sukcesem).
- `Result<T>.Success/Failure` są `internal` (CA1000 łamie build dla statycznych na typie generycznym); publiczne fabryki: `Result.Success<T>(T)` / `Result.Failure<T>(Error)`. Brak operatorów niejawnych.
- `Delete()` nie przyjmuje argumentów (zgodnie z zadaniem) — `DeletedAt/DeletedBy` stempluje interceptor, więc do czasu `SaveChanges` są `null`.
- `Entity.Equals`: encje przejściowe (domyślne `Id`) są równe tylko referencyjnie; typ porównywany bez proxy Castle (lazy-loading EF).
- `Guard` rzuca `DomainException` z kodami `Guard.Null|NullOrWhiteSpace|OutOfRange|Negative|Default` (`ErrorType.Validation`); `AgainstDefault` działa dla ID opartych na `Guid`.
- **Status planu:** commit `b0420b9` oznaczył T02 jako ☑ jeszcze przed implementacją — faktycznie zrobione dopiero w tym zadaniu. T03 było wykonane wcześniej (RAPORT_T03), nie powtarzano.

## Niewykonane (TODO dla kontynuacji)
- Brak.

## Wskazówki dla zależnych zadań
- T17: `DomainEvents` (pole `_domainEvents`) trzeba ignorować w konfiguracji EF (`Ignore(e => e.DomainEvents)`); `Version` → `xmin` (`IsRowVersion`/`HasColumnName("xmin")`); query filter `!IsDeleted` dla `ISoftDeletable`.
- T04–T06: VO jako `sealed record`, zdarzenia dziedziczą po `DomainEvent` i przekazują `TenantId` + czas z agregatu.
