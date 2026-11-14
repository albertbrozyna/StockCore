<!-- Fragment DOMENA.md (D02_sharedkernel). Pełny kontekst: ref/DOMENA.md -->

## 2. Interfejsy i klasy abstrakcyjne SharedKernel

### 2.1 Interfejsy znacznikowe i kontrakty

```csharp
public interface IStronglyTypedId<TValue> where TValue : notnull
{
    TValue Value { get; }
}

public interface IEntity<out TId> where TId : notnull
{
    TId Id { get; }
}

public interface IAggregateRoot { }                       // marker: korzeń agregatu (jedyny punkt wejścia do zapisu)

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
    TenantId TenantId { get; }
}

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

public interface IHasTenant
{
    TenantId TenantId { get; }
}

public interface ICreationAudited
{
    DateTimeOffset CreatedAt { get; }
    UserId? CreatedBy { get; }
}

public interface IAuditable : ICreationAudited
{
    DateTimeOffset? ModifiedAt { get; }
    UserId? ModifiedBy { get; }
}

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
    UserId? DeletedBy { get; }
}

public interface IVersioned
{
    uint Version { get; }          // mapowane na xmin (PostgreSQL) — optymistyczna współbieżność
}
```

### 2.2 Klasy abstrakcyjne

| Klasa | Dziedziczy po / implementuje | Właściwości i zachowanie |
|-------|------------------------------|--------------------------|
| `Entity<TId>` | `IEntity<TId>`, `IHasDomainEvents` | `TId Id { get; protected init; }`; `IReadOnlyCollection<IDomainEvent> DomainEvents`; `protected void Raise(IDomainEvent)`; `void ClearDomainEvents()`; `Equals/GetHashCode` po `Id` i typie (z uwzględnieniem proxy EF); `==`/`!=` |
| `AuditableEntity<TId>` | `Entity<TId>`, `IAuditable`, `IVersioned` | `CreatedAt`, `CreatedBy`, `ModifiedAt`, `ModifiedBy`, `Version`. Wartości ustawia **interceptor EF** (`AuditingInterceptor`) przy `SaveChanges`, używając `IClock` i `ICurrentUser` — encja ich nie ustawia sama |
| `TenantEntity<TId>` | `AuditableEntity<TId>`, `IHasTenant` | `TenantId TenantId { get; private set; }`. Ustawiane przez interceptor z `ITenantContext` przy `Added` (lub jawnie w fabryce agregatu-rodzica). **Niezmienne po utworzeniu**; próba zmiany → wyjątek |
| `SoftDeletableTenantEntity<TId>` | `TenantEntity<TId>`, `ISoftDeletable` | `IsDeleted`, `DeletedAt`, `DeletedBy`; `void Delete()` / `void Restore()`; globalny query filter `!IsDeleted` |
| `ImmutableTenantEntity<TId>` | `Entity<TId>`, `IHasTenant`, `ICreationAudited` | Tylko `CreatedAt`, `CreatedBy`, `TenantId`; **brak** metod mutujących; interceptor blokuje `Modified`/`Deleted` (wyjątek) |
| `ValueObject` | `IEquatable<ValueObject>` | `protected abstract IEnumerable<object?> GetEqualityComponents()`; `Equals`, `GetHashCode`, `==`, `!=`. Preferowane `sealed record` tam, gdzie wystarcza równość strukturalna |
| `Enumeration` | `IComparable<Enumeration>` | `int Id`, `string Name`; `GetAll<T>()`, `FromId<T>(int)`, `FromName<T>(string)`; używana dla enumów z zachowaniem (`DocumentType`, `MovementType`, `DocumentStatus`) |
| `DomainEvent` (record, abstract) | `IDomainEvent` | `EventId = Guid.CreateVersion7()`, `OccurredAt`, `TenantId` — wypełniane w konstruktorze bazowym przez `IClock` przekazany z agregatu |
| `DomainException` | `Exception` | `Error Error`; tworzony z `Error` |

### 2.3 Wynik i błąd

```csharp
public sealed record Error(string Code, string Message, ErrorType Type);   // Code np. "Inventory.InsufficientStock"
public enum ErrorType { Validation, NotFound, Conflict, Forbidden, Unexpected }

public readonly struct Result            { bool IsSuccess; Error Error; }
public readonly struct Result<T>         { bool IsSuccess; T Value; Error Error; }
```
