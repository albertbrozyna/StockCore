# RAPORT_T01 — Strongly-typed ID + konwertery
Status: DONE
Zbudowano: tak (`dotnet build`, 0 warnings) · Testy: przeszły (7 testów jednostkowych)

## Pliki
- `Domain/SharedKernel/Abstractions/IStronglyTypedId.cs` — `IStronglyTypedId<TValue>` (zgodnie z D02).
- `Domain/SharedKernel/Ids/*.cs` — 29 ID z D03, wzorzec z KONWENCJE §5 (`New`, `Empty`, `IsEmpty`, `ToString`).
- `Infrastructure/Persistence/Conventions/StronglyTypedIdValueConverter.cs` — generyczny `ValueConverter<TId, Guid>` (expression trees).
- `Infrastructure/Persistence/Conventions/StronglyTypedIdModelConfigurationExtensions.cs` — `ConfigureStronglyTypedIds(this ModelConfigurationBuilder)`; rejestruje konwertery refleksją.
- `Infrastructure/Persistence/Conventions/StronglyTypedIdScanner.cs` — internal; znajduje typy ID w assembly.
- `Shared/Json/StronglyTypedIdJsonConverter.cs`, `StronglyTypedIdJsonConverterFactory.cs` — System.Text.Json, string GUID (także jako klucz słownika).
- `tests/Warehouse.UnitTests/SharedKernel/Ids/StronglyTypedIdTests.cs` — 7 testów.

## Publiczne API
```csharp
public interface IStronglyTypedId<TValue> where TValue : notnull { TValue Value { get; } }
public readonly record struct SkuId(Guid Value) : IStronglyTypedId<Guid>
{ static SkuId New(); static SkuId Empty { get; } bool IsEmpty { get; } }   // ×29 ID z D03
public sealed class StronglyTypedIdValueConverter<TId> : ValueConverter<TId, Guid> where TId : struct, IStronglyTypedId<Guid>;
public static ModelConfigurationBuilder ConfigureStronglyTypedIds(this ModelConfigurationBuilder builder);
public sealed class StronglyTypedIdJsonConverterFactory : JsonConverterFactory;
```

## Odstępstwa i uwagi
- Brak odstępstw od kontraktu. Poza zakresem: `Sequence` w `StockMovementId` (D03) to kolumna encji `StockMovement`, nie część ID → T10.
- Rejestracja konwerterów EF nie jest jeszcze testowana end-to-end (brak DbContext) — pokryje to T17 (`ConfigureConventions`).
- JSON factory trzeba dodać do `JsonOptions` w `Program.cs` przy pierwszym slice'ie z API.
