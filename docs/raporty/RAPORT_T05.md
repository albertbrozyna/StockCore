# RAPORT_T05 — Value objecty domenowe
Status: DONE
Zbudowano: tak (0 warnings) · Testy: przeszły (15 nowych w `DomainValueObjectTests`; 69 jednostkowych łącznie)

## Pliki
- `Domain/SharedKernel/ValueObjects/` — `SkuCode`, `Barcode`, `LotNumber`, `SerialNumber`, `LocationCode`, `WarehouseCode`, `DocumentNumber`, `ExternalReference`, `ShelfLife`, `LocationCapacity`, `TenantCode`, `TenantSettings`, `DocumentNumberingPolicy`.
- `ValueObjectErrors.cs` (internal, z T04) — dodano `BoundedText(value, max, errorCode, name)`.
- `tests/Warehouse.UnitTests/SharedKernel/ValueObjects/DomainValueObjectTests.cs`.

## Publiczne API
```csharp
// sealed record, prywatny ctor, fabryka Create (DomainException)
SkuCode { const int MaxLength = 50; string Value; static Create(string) }          // UPPER-trim
LotNumber { MaxLength = 50 } · SerialNumber { MaxLength = 100 } · LocationCode { MaxLength = 40 }   // trim, bez zmiany wielkości liter
WarehouseCode { MaxLength = 10 }                                                    // UPPER-trim
TenantCode { string Value; static Create(string) }                                  // ^[a-z0-9-]{3,40}$
Barcode { string Value; BarcodeType Type; static Create(string, BarcodeType) }
DocumentNumber { string Prefix; int Year; long Sequence; string Full; static Create(string prefix, int year, long sequence, int padding) }
ExternalReference { string Source; string Value; static Create(string, string) }
ShelfLife { int TotalDays; int MinRemainingDaysOnReceipt; static Create(int, int) }
LocationCapacity { decimal? MaxWeightKg, MaxVolumeM3, MaxUnits; int? MaxPallets; static Unlimited; static Create(...) }
DocumentNumberingPolicy(bool PerWarehouse, bool ResetYearly, int SequencePadding, string? PrefixOverride) { static Default }   // true, true, 6, null
TenantSettings(bool AllowNegativeStock, AllocationStrategyType DefaultAllocationStrategy, ValuationMethod DefaultValuationMethod,
  TimeSpan? DefaultReservationTtl, CurrencyCode DefaultCurrency, bool RequireLocationOnReceipt, DocumentNumberingPolicy Numbering) { static Default }
```
`TenantSettings.Default`: bez ujemnych stanów, Fifo, WeightedAverage, brak TTL, PLN, lokalizacja przy przyjęciu nieobowiązkowa.
Kody błędów: `Common.Invalid<Typ>` (SkuCode, LotNumber, SerialNumber, LocationCode, WarehouseCode), `Common.InvalidTenantCode`, `Common.InvalidBarcode`, `Common.InvalidDocumentNumber`, `Common.InvalidShelfLife`; braki → `Guard.*`.

## Odstępstwa i uwagi
- Pierwszy argument `DocumentNumber.Create` to cały prefiks (np. `PZ/WH1`), format `{prefix}/{year}/{seq:D<padding>}`; dopuszczalny rok 1900–9999, padding 1–18.
- `LocationCode`: D04 podaje tylko „wzorzec np.”, więc walidowana jest wyłącznie długość (brak ograniczenia znaków). `LotNumber`/`SerialNumber`/`LocationCode` nie zmieniają wielkości liter; `WarehouseCode` jest UPPER (D04 tego nie mówi — spójne z `SkuCode`/`UnitCode`). **Pytanie do architekta.**
- `SkuCode` nie wymusza braku spacji *wewnątrz* kodu — tylko trim na brzegach (D04).
- `TenantSettings`/`DocumentNumberingPolicy` to rekordy pozycyjne (publiczny ctor, `with` dozwolone), bez walidacji `SequencePadding` — walidacja w `Tenant` (T08).
- Barcode: algorytm modulo 10 (wagi 3,1 od prawej); długości EAN-13=13, EAN-8=8, UPC-A=12.

## Niewykonane (TODO dla kontynuacji)
- Brak.

## Wskazówki dla zależnych zadań
- Konwersje EF (`HasConversion`/`OwnsOne`) dla tych VO — T18–T20. `TenantSettings` → `OwnsOne` (zagnieżdżony `Numbering`).
