# Konwencje projektu Warehouse (OBOWIĄZKOWE w każdym zadaniu)

Ten plik jest **kontraktem wspólnym** dla wszystkich czatów. Jeśli coś tu jest sprzeczne z `domena/*.md`, **wygrywa ten plik** (to są rozstrzygnięcia otwartych decyzji z §13).

## 1. Rozwiązanie i przestrzenie nazw

Jeden projekt Web API: `src/Warehouse.Api` (root namespace `Warehouse.Api`). Testy w osobnych projektach.

```text
src/Warehouse.Api/
├── Domain/
│   ├── SharedKernel/
│   │   ├── Abstractions/      # interfejsy (IEntity, IHasTenant, IAuditable, ...)
│   │   ├── Ids/               # WSZYSTKIE strongly-typed ID
│   │   ├── ValueObjects/      # WSZYSTKIE value objecty
│   │   ├── Enums/             # WSZYSTKIE enumy i smart enums (Enumeration)
│   │   └── (Entity.cs, AuditableEntity.cs, TenantEntity.cs, ... Result.cs, Error.cs, Guard.cs)
│   ├── Tenancy/  Catalog/  Locations/  Inventory/  Documents/  Fulfillment/  Stocktaking/  Alerts/
│   │   └── <kontekst>/Events/ , <kontekst>/Services/ (interfejsy serwisów domenowych)
├── Application/
│   └── Abstractions/          # IClock, ICurrentUser, ITenantContext, IUnitOfWork, CQRS, pipeline
├── Features/                  # vertical slices (na razie puste)
├── Infrastructure/
│   └── Persistence/           # WarehouseDbContext, Conventions/, Interceptors/, Configurations/<Kontekst>/, Migrations/
└── Shared/
```
Namespace = ścieżka folderu (np. `Warehouse.Api.Domain.Inventory.Events`).

## 2. Rozstrzygnięcia decyzji (K-1 … K-10)

| # | Decyzja |
|---|---------|
| K-1 | Zdarzenia domenowe w `Entity<TId>` (`Raise`, `DomainEvents`, `ClearDomainEvents`). |
| K-2 | **Wszystkie** ID, value objecty i enumy (w tym smart enums) leżą w `Domain/SharedKernel/…` — kontekstowe foldery zawierają tylko encje, zdarzenia i serwisy. Zdejmuje to zależności między kontekstami. |
| K-3 | Dokumenty i ich linie: EF **TPH**. |
| K-4 | EF Core + Npgsql do zapisu. Domena **nie** referuje EF ani ASP.NET (żadnych atrybutów EF w domenie). |
| K-5 | Własny lekki dispatcher CQRS (bez MediatR). |
| K-6 | Księgowanie ruchów **synchronicznie** w handlerze zatwierdzenia dokumentu (jedna transakcja). |
| K-7 | Multi-tenancy: shared schema + `TenantId`, globalny query filter, `TenantId` pierwszy w indeksach. RLS opcjonalnie później. |
| K-8 | Jednostki miary: `UnitOfMeasure` per tenant. |
| K-9 | **Fabryki `Create(...)` rzucają `DomainException(Error)` przy naruszeniu niezmiennika** (nie zwracają `Result<T>`). `Result`/`Result<T>` używane są w warstwie Application (handlery). |
| K-10 | Cykl zależności Inventory ↔ Documents zerwany: `IStockPostingService.Post(DocumentId documentId, IReadOnlyList<MovementDraft> drafts, IReadOnlyDictionary<StockLevelKey, StockLevel> affectedLevels, PostingContext context)`. `MovementDraft`, `PostingResult`, `PostingContext`, `StockLevelKey` leżą w `Domain/Inventory/Posting/`. `WarehouseDocument.BuildMovements()` zwraca `IReadOnlyList<MovementDraft>`. |

## 3. Dozwolone zależności typów między kontekstami

`SharedKernel` ← wszystko. Dalej (strzałka = „może używać typów z"):
`Catalog`, `Locations`, `Tenancy` → tylko SharedKernel · `Inventory` → SharedKernel (+ `Catalog`/`Locations` tylko jeśli konieczne, preferuj samo ID) · `Documents` → `Inventory` (MovementDraft), `Catalog` · `Fulfillment` → `Inventory`, `Locations` · `Stocktaking` → `Inventory`, `Documents` · `Alerts` → SharedKernel.
Odwołania do agregatów innych kontekstów **wyłącznie przez ID**. Zdarzenia domenowe używają tylko ID, enumów i prymitywów (`decimal`, `string`), **bez** value objectów.

## 4. Styl kodu

- C# 14, `#nullable enable`, file-scoped namespaces, **jeden typ publiczny na plik**, nazwa pliku = nazwa typu.
- Klasy `sealed`, o ile nie są abstrakcyjne lub nie wymaga tego EF (TPH: klasy bazowe `abstract`, pochodne `sealed`).
- Value objecty i zdarzenia: `sealed record` / `readonly record struct`. Brak publicznych setterów w encjach (`private set` / `init`), prywatny bezparametrowy konstruktor dla EF, publiczna statyczna fabryka `Create`.
- Kolekcje: prywatne `List<T>`, publicznie `IReadOnlyCollection<T>`.
- Czas wyłącznie przez parametry (`DateTimeOffset now`) lub `IClock`; **zakaz** `DateTime.Now/UtcNow`.
- Domena bez zależności NuGet (poza BCL). Komentarze XML krótkie, po angielsku, tylko na publicznym API.
- Walidacja argumentów przez `Guard` (SharedKernel) lub `DomainException(new Error(code, message, ErrorType.Validation))`. Kody błędów: `<Kontekst>.<Nazwa>` np. `Inventory.InsufficientStock`.

## 5. Strongly-typed ID — wzorzec (identyczny dla wszystkich)

```csharp
namespace Warehouse.Api.Domain.SharedKernel.Ids;

public readonly record struct SkuId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkuId New() => new(Guid.CreateVersion7());
    public static SkuId Empty => new(Guid.Empty);
    public bool IsEmpty => Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
```

## 6. Zasady pracy na czacie

1. Rób **tylko** zakres zadania. Nie poprawiaj cudzych plików; rozbieżności zgłoś w raporcie (sekcja „Odstępstwa i uwagi").
2. Nie wymyślaj nowych właściwości — brakujące/sprzeczne rzeczy opisz w raporcie jako pytanie do architekta.
3. Jeśli masz dostęp do `dotnet` — zbuduj (`dotnet build`) i uruchom testy; jeśli nie — przeczytaj kod pod kątem kompilacji (brakujące `using`, typy, nazwy).
4. Na końcu **zawsze** wygeneruj `RAPORT_Txx.md` wg `02_SZABLON_RAPORTU.md`. To raport — nie kod — jest wejściem dla następnych czatów.
5. Kończą się tokeny? Dokończ bieżący plik, **nie zaczynaj kolejnego**, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików (stanowi on gotowy brief na kontynuację).
