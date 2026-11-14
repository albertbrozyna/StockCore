<!-- Fragment DOMENA.md (D04_value_objects). Pełny kontekst: ref/DOMENA.md -->

## 4. Value Objects

Wszystkie `sealed`, niemutowalne, z walidacją w fabryce `Create(...)`. Mapowane jako *owned types* / konwertery.

| Value Object | Właściwości | Reguły / operacje |
|--------------|-------------|-------------------|
| `Quantity` | `decimal Amount`, `UnitCode Unit` | `Amount` może być ujemne tylko w kontekście `QuantityDelta`; operacje `Add`, `Subtract`, `Negate`, `Multiply(decimal)`, `IsZero`, `IsPositive`; działania na różnych jednostkach → `DomainException`; zaokrąglenie wg `UnitOfMeasure.DecimalPlaces` |
| `UnitCode` | `string Value` (1–10 znaków, UPPER) | np. `PCS`, `KG`, `CTN`, `PAL`, `L` |
| `Money` | `decimal Amount`, `CurrencyCode Currency` | `Add/Subtract` tylko w tej samej walucie; `Multiply(decimal)`; `Zero(currency)` |
| `CurrencyCode` | `string Value` (ISO 4217, 3 litery) | walidacja po liście |
| `SkuCode` | `string Value` (≤ 50, bez spacji na brzegach, UPPER-trim) | unikalny w obrębie tenanta |
| `Barcode` | `string Value`, `BarcodeType Type` | walidacja sumy kontrolnej dla `Ean13`, `Ean8`, `UpcA`; `Code128`, `Gs1128`, `Custom` bez sumy |
| `LotNumber` | `string Value` (≤ 50) | unikalny w obrębie `(TenantId, SkuId)` |
| `SerialNumber` | `string Value` (≤ 100) | unikalny w obrębie `(TenantId, SkuId)` |
| `LocationCode` | `string Value` (≤ 40, wzorzec np. `A-01-03-2`) | unikalny w obrębie `(TenantId, WarehouseId)` |
| `WarehouseCode` | `string Value` (≤ 10) | unikalny w obrębie tenanta |
| `DocumentNumber` | `string Prefix`, `int Year`, `long Sequence`, `string Full` | `Full` np. `PZ/WH1/2026/000123`; tworzony wyłącznie przez `IDocumentNumberGenerator` |
| `ExternalReference` | `string Source`, `string Value` | np. `("ERP-ORDER","ZK/12/2026")`; do korelacji z zewnętrznymi systemami |
| `Address` | `Street`, `BuildingNo`, `ApartmentNo?`, `PostalCode`, `City`, `Region?`, `CountryCode` | |
| `ContactInfo` | `string? Email`, `string? Phone`, `string? PersonName` | walidacja formatu e-mail |
| `TaxId` | `string CountryCode`, `string Number` | walidacja NIP dla `PL` |
| `Dimensions` | `decimal Length`, `Width`, `Height`, `LengthUnit Unit` | `Volume` (computed) |
| `Weight` | `decimal Value`, `WeightUnit Unit` | konwersja do kg |
| `ShelfLife` | `int TotalDays`, `int MinRemainingDaysOnReceipt` | używany przy przyjęciu partii z datą ważności |
| `LocationCapacity` | `decimal? MaxWeightKg`, `decimal? MaxVolumeM3`, `decimal? MaxUnits`, `int? MaxPallets` | sprawdzane przez `ILocationCapacityPolicy` |
| `DateRange` | `DateOnly From`, `DateOnly To` | `From <= To` |
| `TenantCode` | `string Value` (slug, `[a-z0-9-]{3,40}`) | unikalny globalnie |
| `TenantSettings` | `bool AllowNegativeStock`, `AllocationStrategyType DefaultAllocationStrategy`, `ValuationMethod DefaultValuationMethod`, `TimeSpan? DefaultReservationTtl`, `CurrencyCode DefaultCurrency`, `bool RequireLocationOnReceipt`, `DocumentNumberingPolicy Numbering` | VO osadzony w `Tenant` |
| `DocumentNumberingPolicy` | `bool PerWarehouse`, `bool ResetYearly`, `int SequencePadding`, `string? PrefixOverride` | |
