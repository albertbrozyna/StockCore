<!-- Fragment DOMENA.md (D06_2_catalog). Pełny kontekst: ref/DOMENA.md -->

### 6.2 Kontekst: Catalog

#### `UnitOfMeasure` · `SoftDeletableTenantEntity<UnitOfMeasureId>`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `UnitCode` | unikalny w tenancie |
| `Name` | `string` | |
| `Kind` | `UnitKind` | |
| `DecimalPlaces` | `int` | dopuszczalna precyzja (0 dla szt.) |

#### `SkuCategory` · `SoftDeletableTenantEntity<SkuCategoryId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `string` | |
| `Name` | `string` | |
| `ParentId` | `SkuCategoryId?` | drzewo |
| `Path` | `string` | materialized path (np. `/1/4/9/`) |

#### `Sku` · `SoftDeletableTenantEntity<SkuId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `SkuCode` | unikalny w tenancie |
| `Name` | `string` | |
| `Description` | `string?` | |
| `CategoryId` | `SkuCategoryId?` | |
| `BaseUnit` | `UnitCode` | jednostka bazowa — w niej liczy się cały stan i ledger |
| `TrackingMode` | `TrackingMode` | partie / numery seryjne |
| `RequiresExpiryDate` | `bool` | wymusza `ExpiryDate` na przyjęciu partii |
| `ShelfLife` | `ShelfLife?` | |
| `Dimensions` | `Dimensions?` | |
| `Weight` | `Weight?` | |
| `AllocationStrategy` | `AllocationStrategyType?` | `null` = domyślna z `TenantSettings` |
| `ValuationMethod` | `ValuationMethod?` | `null` = domyślna z `TenantSettings` |
| `ReorderPoint` | `decimal?` | próg alertu `LowStock` (w jedn. bazowej) |
| `MinStockLevel` | `decimal?` | |
| `DefaultSupplierId` | `ContractorId?` | |
| `VariantOfSkuId` | `SkuId?` | wariant produktu |
| `Attributes` | `IReadOnlyDictionary<string,string>` | JSONB — dowolne atrybuty/warianty |
| `IsActive` | `bool` | |
| `Barcodes` | `IReadOnlyCollection<SkuBarcode>` | |
| `UnitConversions` | `IReadOnlyCollection<SkuUnitConversion>` | |

Metody: `Create`, `Rename`, `ChangeTrackingMode` (zablokowane gdy istnieje stan), `AddBarcode`, `RemoveBarcode`, `SetPrimaryBarcode`, `AddUnitConversion`, `RemoveUnitConversion`, `ConvertToBase(Quantity)`, `ConvertFromBase(decimal, UnitCode)`, `Activate`, `Deactivate`, `Delete`.
Zdarzenia: `SkuCreated`, `SkuDeactivated`, `SkuTrackingModeChanged`.
Niezmienniki: `BaseUnit` niezmienna po pierwszym ruchu; tylko jeden `Barcode` z `IsPrimary`; przelicznik do jednostki bazowej `Factor > 0`; `RequiresExpiryDate` wymaga `TrackingMode ∈ {Lot, LotAndSerial}`.

#### `SkuBarcode` · `TenantEntity<SkuBarcodeId>` (zależna od `Sku`)

`SkuId`, `Barcode Barcode`, `UnitCode Unit` (opakowanie, którego dotyczy kod), `bool IsPrimary`.

#### `SkuUnitConversion` · `TenantEntity<SkuUnitConversionId>` (zależna od `Sku`)

`SkuId`, `UnitCode FromUnit`, `decimal Factor` (ile jedn. bazowych w 1 `FromUnit`; np. `CTN → 12`), `bool IsDefaultForReceipt`, `bool IsDefaultForIssue`.

#### `Contractor` · `SoftDeletableTenantEntity<ContractorId>`, `IAggregateRoot`

| Właściwość | Typ | Opis |
|------------|-----|------|
| `Code` | `string` | unikalny w tenancie |
| `Name` | `string` | |
| `Kind` | `ContractorKind` (flags) | dostawca / odbiorca / oba |
| `TaxId` | `TaxId?` | |
| `DefaultCurrency` | `CurrencyCode?` | |
| `PaymentTermsDays` | `int?` | |
| `IsActive` | `bool` | |
| `Addresses` | `IReadOnlyCollection<ContractorAddress>` | |
| `Contacts` | `IReadOnlyCollection<ContractorContact>` | |
| `ExternalReferences` | `IReadOnlyCollection<ExternalReference>` | |

`ContractorAddress` (zależna): `Address Address`, `AddressType Type (Billing, Shipping, Registered)`, `bool IsDefault`.
`ContractorContact` (zależna): `ContactInfo Contact`, `string? Role`.
Metody: `Create`, `Update`, `AddAddress`, `AddContact`, `Activate`, `Deactivate`, `Delete`.
