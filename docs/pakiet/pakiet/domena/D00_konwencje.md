<!-- Fragment DOMENA.md (D00_konwencje). Pełny kontekst: ref/DOMENA.md -->

# Warehouse — specyfikacja modelu domenowego

> Dokument dla architekta / implementatora. Opisuje **wszystkie klasy domenowe, ich właściwości, interfejsy, klasy abstrakcyjne i hierarchię dziedziczenia**.
> Nie zawiera implementacji — tylko kontrakt. Uzupełnia `ZALOZENIA.md`.
> Stack: .NET 10 / C# 14 · Vertical Slice w jednym projekcie Web API · DDD · PostgreSQL · **multi-tenant (shared DB, `TenantId`)**.

---

## 0. Konwencje (obowiązujące w całym dokumencie)

| Temat | Zasada |
|-------|--------|
| Nazewnictwo | Identyfikatory w kodzie po angielsku; polskie odpowiedniki w słowniku `docs/domain/glossary.md` |
| Identyfikatory | **Strongly-typed ID**: `readonly record struct XxxId(Guid Value) : IStronglyTypedId<Guid>`; generowane `Guid.CreateVersion7()` |
| Czas | `DateTimeOffset` (UTC) dla momentów, `DateOnly` dla dat kalendarzowych (data dokumentu, data ważności). Nigdy `DateTime.Now` — tylko `IClock` / `TimeProvider` |
| Ilości | `decimal`, skala 4 (`numeric(18,4)`); zawsze w `Quantity` (wartość + jednostka) |
| Pieniądze | `Money` (`decimal` skala 4 + `CurrencyCode`) |
| Nullability | `#nullable enable`; `?` = opcjonalne biznesowo |
| Konstruktory | Encje: `private` bezparametrowy (EF) + publiczna **fabryka statyczna** `Create(...)` zwracająca `Result<T>` lub rzucająca `DomainException`. Brak publicznych setterów |
| Kolekcje | Pole `private readonly List<T>`, publicznie `IReadOnlyCollection<T>` |
| Zdarzenia | Agregat woła `Raise(IDomainEvent)`; dispatch po `SaveChanges` (patrz §9) |
| Błędy | Naruszenie niezmiennika → `DomainException(Error)`; w warstwie slice mapowane na `ProblemDetails` |
| Multi-tenancy | **Każda** encja poza `Tenant` dziedziczy po `TenantEntity<TId>` (lub `ImmutableTenantEntity<TId>`), ma `TenantId`, globalny query filter w EF, `TenantId` jest pierwszą kolumną każdego indeksu |
| Strefy czasowe | Przechowujemy UTC; `Tenant.TimeZone` / `Warehouse.TimeZone` używane tylko do prezentacji i granic dnia |
