# RAPORT_T08 — Tenancy + Catalog (agregaty)
Status: DONE
Zbudowano: tak (`dotnet build`, 0 warnings) · Testy: przeszły (17 nowych w `TenancyTests` i `CatalogTests`; 106 jednostkowych łącznie)

## Pliki
- `Domain/Tenancy/Tenant.cs`, `DocumentNumberSequence.cs`
- `Domain/Catalog/` — `UnitOfMeasure`, `SkuCategory`, `Sku`, `SkuBarcode`, `SkuUnitConversion`, `Contractor`, `ContractorAddress`, `ContractorContact`; internal `CatalogText` (walidacja tekstu i fabryka błędów)
- `Domain/Catalog/Services/IUnitConverter.cs`, `UnitConverter.cs`
- `Domain/SharedKernel/Ids/ContractorAddressId.cs`, `ContractorContactId.cs`, `Enums/AddressType.cs` (patrz Odstępstwa)
- `tests/Warehouse.UnitTests/Domain/TenancyTests.cs`, `CatalogTests.cs`; `StronglyTypedIdTests` — liczba ID 29 → 31

## Publiczne API
```csharp
// Warehouse.Api.Domain.Tenancy
public sealed class Tenant : AuditableEntity<TenantId>, IAggregateRoot
{ TenantCode Code; string Name; TenantStatus Status; string TimeZone; TenantSettings Settings; TaxId? TaxId; Address? Address; DateTimeOffset? ActivatedAt, SuspendedAt;
  static Tenant Create(TenantCode, string name, string timeZone, TenantSettings, DateTimeOffset now, TaxId? = null, Address? = null);
  void Rename(string); void UpdateSettings(TenantSettings); void Suspend(string reason, DateTimeOffset now); void Activate(DateTimeOffset now); void Deactivate(); }
public sealed class DocumentNumberSequence : TenantEntity<DocumentNumberSequenceId>
{ DocumentType DocumentType; WarehouseId? WarehouseId; int? Year; string Prefix; long LastValue;
  static DocumentNumberSequence Create(DocumentType, string prefix, WarehouseId? = null, int? year = null); long Next(); }

// Warehouse.Api.Domain.Catalog
public sealed class UnitOfMeasure : SoftDeletableTenantEntity<UnitOfMeasureId> { UnitCode Code; string Name; UnitKind Kind; int DecimalPlaces; static Create(UnitCode, string, UnitKind, int decimalPlaces /*0..6*/); }
public sealed class SkuCategory : SoftDeletableTenantEntity<SkuCategoryId>, IAggregateRoot { string Code, Name; SkuCategoryId? ParentId; string Path; static Create(string code, string name, SkuCategory? parent = null); }
public sealed class Sku : SoftDeletableTenantEntity<SkuId>, IAggregateRoot
{ SkuCode Code; string Name; string? Description; SkuCategoryId? CategoryId; UnitCode BaseUnit; TrackingMode TrackingMode; bool RequiresExpiryDate; ShelfLife? ShelfLife; Dimensions? Dimensions; Weight? Weight;
  AllocationStrategyType? AllocationStrategy; ValuationMethod? ValuationMethod; decimal? ReorderPoint, MinStockLevel; ContractorId? DefaultSupplierId; SkuId? VariantOfSkuId;
  IReadOnlyDictionary<string,string> Attributes; bool IsActive; IReadOnlyCollection<SkuBarcode> Barcodes; IReadOnlyCollection<SkuUnitConversion> UnitConversions;
  static Sku Create(TenantId, SkuCode, string name, UnitCode baseUnit, TrackingMode, DateTimeOffset now, /* opcjonalne: description, categoryId, requiresExpiryDate, shelfLife, dimensions, weight, allocationStrategy, valuationMethod, reorderPoint, minStockLevel, defaultSupplierId, variantOfSkuId, attributes */);
  void Rename(string); void ChangeTrackingMode(TrackingMode, bool hasStock); void AddBarcode(Barcode, UnitCode, bool isPrimary = false); void RemoveBarcode(Barcode); void SetPrimaryBarcode(Barcode);
  void AddUnitConversion(UnitCode from, decimal factor, bool isDefaultForReceipt = false, bool isDefaultForIssue = false); void RemoveUnitConversion(UnitCode);
  Quantity ConvertToBase(Quantity); Quantity ConvertFromBase(decimal baseAmount, UnitCode target); void Activate(); void Deactivate(DateTimeOffset now); /* + Delete() z bazy */ }
public sealed class SkuBarcode : TenantEntity<SkuBarcodeId> { SkuId SkuId; Barcode Barcode; UnitCode Unit; bool IsPrimary; }          // tylko przez Sku
public sealed class SkuUnitConversion : TenantEntity<SkuUnitConversionId> { SkuId SkuId; UnitCode FromUnit; decimal Factor; bool IsDefaultForReceipt, IsDefaultForIssue; }
public sealed class Contractor : SoftDeletableTenantEntity<ContractorId>, IAggregateRoot
{ string Code, Name; ContractorKind Kind; TaxId? TaxId; CurrencyCode? DefaultCurrency; int? PaymentTermsDays; bool IsActive; Addresses; Contacts; ExternalReferences;
  static Create(string code, string name, ContractorKind, TaxId? = null, CurrencyCode? = null, int? paymentTermsDays = null);
  void Update(string name, ContractorKind, TaxId?, CurrencyCode?, int? paymentTermsDays); void AddAddress(Address, AddressType, bool isDefault = false); void AddContact(ContactInfo, string? role = null);
  void AddExternalReference(ExternalReference); void Activate(); void Deactivate(); }
public sealed class ContractorAddress : TenantEntity<ContractorAddressId> { ContractorId ContractorId; Address Address; AddressType Type; bool IsDefault; }
public sealed class ContractorContact : TenantEntity<ContractorContactId> { ContractorId ContractorId; ContactInfo Contact; string? Role; }
// Warehouse.Api.Domain.Catalog.Services
public interface IUnitConverter { Quantity ToBase(Sku, Quantity); Quantity FromBase(Sku, decimal baseAmount, UnitCode target); }
public sealed class UnitConverter : IUnitConverter
```
Kody błędów: `Tenancy.InvalidName|InvalidTimeZone|InvalidReservationTtl|NotActive|AlreadyActive|AlreadyDeactivated`; `Catalog.InvalidName|InvalidCode|ExpiryRequiresLotTracking|TrackingModeLocked|DuplicateBarcode|BarcodeNotFound|InvalidConversionFactor|ConversionFromBaseUnit|DuplicateConversion|ConversionNotFound|NoUnitConversion|InvalidContractorKind|DuplicateExternalReference`; braki wartości → `Guard.*`.

## Odstępstwa i uwagi
- **Brakujące typy SharedKernel** (K-2): D06 używa `AddressType` i encji `ContractorAddress`/`ContractorContact`, ale D03/D05 nie definiują ich ID ani enuma. Dodano `AddressType {Billing, Shipping, Registered}`, `ContractorAddressId`, `ContractorContactId` oraz poprawiono test liczby ID (29 → 31). **Do potwierdzenia.**
- **Zdarzenia z D06, których nie ma w T06:** `TenantActivated`, `SkuTrackingModeChanged`. Nie dodawano ich (T06 ma 28 zdarzeń, test pilnuje liczby), więc `Tenant.Activate` i `Sku.ChangeTrackingMode` nic nie zgłaszają. **Pytanie do architekta:** dodać je w osobnym zadaniu?
- `Sku.Create` i `Tenant.Create` zgłaszają zdarzenia z `TenantId`/`now`. Nic w T17 nie uzupełnia `TenantId` zdarzeń, więc `Sku.Create` przyjmuje `TenantId` i woła `SetTenant` (mechanizm z RAPORT_T02). Pozostałe encje dostają tenant od interceptora; encje zależne (`SkuBarcode` itd.) też.
- `ChangeTrackingMode(mode, bool hasStock)`: agregat nie zna stanów (Inventory), więc wołający (handler) przekazuje, czy stan istnieje. `BaseUnit` jest niezmienna, bo nie ma metody jej zmiany.
- `Deactivate(now)` jest idempotentne i zgłasza `SkuDeactivated` tylko przy zmianie. `Activate()` nie zgłasza zdarzenia.
- `AddExternalReference` nie jest w D06, ale bez niej kolekcja `ExternalReferences` byłaby nie do wypełnienia. Drobny dodatek.
- `Sku.AddBarcode` nie wymusza, by `Unit` kodu była jednostką bazową lub miała przelicznik (D06 tego nie wymaga).
- `SkuCategory.Path`: identyfikatory to Guid, więc path to `/{guid N}/…/` (nie `/1/4/9/` z D06). Brak metod poza `Create` (D06 ich nie podaje).
- `UnitOfMeasure.DecimalPlaces` ograniczone do 0–6 (D06 nie podaje górnej granicy). Zaokrąglanie `Quantity` wg `DecimalPlaces` (uwaga z RAPORT_T04) nie jest w zakresie T08.
- `Tenant.TimeZone` walidowane przez `TimeZoneInfo.TryFindSystemTimeZoneById` — wymaga tzdata w obrazie runtime. `Suspend(reason, now)`: powód jest wymagany, ale nie przechowywany (`TenantSuspended` go nie ma). `SequencePadding` 1–18 walidowany w `Tenant`, jak zalecał RAPORT_T05.
- `DocumentNumberSequence.Next()` nie blokuje wiersza — blokada (`FOR UPDATE`) lub `xmin` + retry to zadanie warstwy persystencji.

## Niewykonane (TODO dla kontynuacji)
- Brak.

## Wskazówki dla zależnych zadań
- T18: `Attributes` mapować na JSONB (pole `_attributes`); `Barcodes`/`UnitConversions`/`Addresses`/`Contacts` jako kolekcje przez pola prywatne; `ExternalReferences` jako owned collection/JSONB; unikalne indeksy `(TenantId, Code)` dla Sku, UnitOfMeasure, Contractor oraz `(TenantId, DocumentType, WarehouseId, Year)` dla sekwencji.
- T15/T16: korzystać z `IUnitConverter` (implementacja bezstanowa, rejestrować jako singleton).
