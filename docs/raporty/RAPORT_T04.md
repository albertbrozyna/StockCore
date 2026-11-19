# RAPORT_T04 — Value objecty ogólne
Status: DONE
Zbudowano: tak (0 warnings) · Testy: przeszły (16 nowych w `GeneralValueObjectTests`; 48 jednostkowych łącznie)

## Pliki
- `Domain/SharedKernel/ValueObjects/` — `Quantity`, `UnitCode`, `Money`, `CurrencyCode`, `Dimensions`, `Weight`, `DateRange`, `Address`, `ContactInfo`, `TaxId`; internal `ValueObjectErrors` (pomocnik do błędów walidacji).
- `tests/Warehouse.UnitTests/SharedKernel/ValueObjects/GeneralValueObjectTests.cs`.

## Publiczne API
```csharp
// namespace Warehouse.Api.Domain.SharedKernel.ValueObjects — wszystkie sealed record, prywatny konstruktor, fabryka Create (rzuca DomainException)
public sealed record UnitCode { const int MaxLength = 10; string Value; static UnitCode Create(string) }
public sealed record CurrencyCode { string Value; static CurrencyCode Pln; static CurrencyCode Create(string) }
public sealed record Quantity { decimal Amount; UnitCode Unit; bool IsZero; bool IsPositive; static Create(decimal, UnitCode); static Zero(UnitCode);
  Quantity Add(Quantity); Subtract(Quantity); Negate(); Multiply(decimal) }
public sealed record Money { decimal Amount; CurrencyCode Currency; static Create(decimal, CurrencyCode); static Zero(CurrencyCode); Add; Subtract; Multiply(decimal) }
public sealed record Dimensions { decimal Length, Width, Height; LengthUnit Unit; decimal Volume; static Create(decimal, decimal, decimal, LengthUnit) }
public sealed record Weight { decimal Value; WeightUnit Unit; decimal ToKilograms(); static Create(decimal, WeightUnit) }
public sealed record DateRange { DateOnly From, To; static Create(DateOnly, DateOnly) }
public sealed record Address { Street, BuildingNo, ApartmentNo?, PostalCode, City, Region?, CountryCode; static Create(street, buildingNo, apartmentNo?, postalCode, city, region?, countryCode) }
public sealed record ContactInfo { string? Email, Phone, PersonName; static Create(string?, string?, string?) }
public sealed record TaxId { string CountryCode; string Number; static Create(string countryCode, string number) }
```
Kody błędów: `Common.UnitMismatch`, `Common.CurrencyMismatch`, `Common.InvalidUnitCode`, `Common.InvalidCurrency`, `Common.InvalidDateRange`, `Common.InvalidCountryCode`, `Common.InvalidEmail`, `Common.InvalidTaxId`; braki wartości → `Guard.*`.

## Odstępstwa i uwagi
- **`with` omija walidację** (cecha `record`): `x with { Amount = … }` nie przechodzi przez `Create`. Dla `Quantity`/`Money` to akceptowalne; w razie potrzeby zamienić na `ValueObject`.
- `Quantity`: brak zaokrąglania wg `UnitOfMeasure.DecimalPlaces` (D04) — zależy od `UnitOfMeasure` (T08); zaokrągla wołający lub osobna metoda w T08. Ujemne wartości dozwolone (delta), brak osobnego `QuantityDelta`.
- `CurrencyCode`: lista wbudowana = 6 wymaganych + SEK, NOK, DKK, HUF, RON, BGN, UAH, JPY, CAD, AUD, CNY. Dodano `CurrencyCode.Pln` (domyślna waluta) — drobny dodatek.
- `Dimensions.Volume` jest w sześciennych jednostkach `Unit` (bez konwersji do m³); `LocationCapacity.MaxVolumeM3` (T05) będzie wymagać konwersji — brak metody w zakresie T04, **pytanie do architekta** czy dodać `VolumeInCubicMeters()`.
- `Dimensions`/`Weight` dopuszczają 0 (tylko ujemne odrzucane).
- `ContactInfo`: pusty/whitespace → `null`; e-mail walidowany `MailAddress` + kropka w hoście; żadne pole nie jest wymagane.
- `TaxId` PL: toleruje myślniki, spacje i prefiks `PL`; zapisuje 10 cyfr. `Address`/`TaxId` kraj: 2 litery, UPPER (bez listy ISO).
- `Money`/`Quantity` implementują równość strukturalną rekordu (`decimal` porównuje wartość, `1.0m == 1.00m`).

## Niewykonane (TODO dla kontynuacji)
- Brak.

## Wskazówki dla zależnych zadań
- T05: te same wzorce (`sealed record`, prywatny ctor, `Create`, `ValueObjectErrors` jest `internal` — dostępny w całym assembly).
- T17/T19: owned types / konwertery EF dla tych VO (Quantity → 2 kolumny itd.).
