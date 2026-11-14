# T07 — Abstrakcje aplikacyjne (Application/Abstractions)

**Fala:** 3 · **Zależy od:** T01, T02 · **Szacunek wyjścia:** ~8k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D08_abstrakcje_aplikacyjne.md`
- `domena/D02_sharedkernel.md` (Result/Error)
- **Raporty** zależności: `RAPORT_T01.md`, `RAPORT_T02.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T07 — Abstrakcje aplikacyjne (Application/Abstractions).
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Application/Abstractions/: IClock, ICurrentUser, ITenantContext, IUnitOfWork, IDomainEventDispatcher, IIntegrationEventPublisher, IIdempotencyService (+ IdempotencyResult), IAuditLogWriter
- Application/Abstractions/Messaging/: ICommand<T>, IQuery<T>, ICommandHandler, IQueryHandler, IPipelineBehavior, IDispatcher (+ prosta implementacja `Dispatcher` rozwiązująca handlery z DI i składająca pipeline)
- Application/Behaviors/: ValidationBehavior (FluentValidation), TenantGuardBehavior, TransactionBehavior, LoggingBehavior (szkielety z poprawnym kontraktem)
- Application/DependencyInjection.cs — `AddApplication()` skanujący handlery

## Zakres i reguły szczegółowe

- Same interfejsy + dispatcher + behaviors; **bez** implementacji ICurrentUser/ITenantContext (to Infrastructure/API)

## Poza zakresem

- Implementacje EF
- Endpointy

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T07.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~8k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T07b`).
