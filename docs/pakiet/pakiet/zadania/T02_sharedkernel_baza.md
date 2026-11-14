# T02 — SharedKernel — klasy bazowe i abstrakcje

**Fala:** 2 · **Zależy od:** T01 · **Szacunek wyjścia:** ~9k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D02_sharedkernel.md`
- `domena/D01_hierarchia.md` (§1.1, §1.3)
- **Raporty** zależności: `RAPORT_T01.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T02 — SharedKernel — klasy bazowe i abstrakcje.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/SharedKernel/Abstractions/: IEntity, IAggregateRoot, IDomainEvent, IHasDomainEvents, IHasTenant, ICreationAudited, IAuditable, ISoftDeletable, IVersioned
- Domain/SharedKernel/: Entity<TId>, AuditableEntity<TId>, TenantEntity<TId>, SoftDeletableTenantEntity<TId>, ImmutableTenantEntity<TId>
- ValueObject, Enumeration, DomainEvent (abstract record), DomainException, Error, ErrorType, Result, Result<T>, Guard

## Zakres i reguły szczegółowe

- Settery audytu/TenantId: `protected`/`internal set`, ustawiane przez interceptor (T17) — udostępnij do tego jawny, udokumentowany mechanizm (np. `internal void SetTenant(TenantId)` + `[InternalsVisibleTo]` albo publiczna metoda `Stamp...` oznaczona jako infrastrukturalna) i opisz go w raporcie
- `TenantEntity.TenantId` niezmienne po ustawieniu (wyjątek przy próbie zmiany)
- Entity: Equals/GetHashCode po Id i typie, operatory ==, !=
- `Guard`: AgainstNull, AgainstNullOrWhiteSpace, AgainstOutOfRange, AgainstNegative, AgainstDefault (dla ID)
- Enumeration: Id, Name, GetAll<T>(), FromId<T>(), FromName<T>(), CompareTo, Equals

## Poza zakresem

- Konkretne encje, VO, enumy konkretne
- EF / interceptory

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T02.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~9k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T02b`).
