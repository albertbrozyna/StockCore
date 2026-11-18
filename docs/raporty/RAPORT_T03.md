# RAPORT_T03 — Enumeracje (zwykłe i smart enums)
Status: DONE
Zbudowano: tak (0 warnings) · Testy: przeszły (14 testów w `SmartEnumTests`)

## Pliki
- `Domain/SharedKernel/Enums/*.cs` — 27 zwykłych enumów z D05 §5.2 (`ContractorKind` z `[Flags]`) + `DocumentType`, `MovementType`, `DocumentStatus` (smart enums).
- `Domain/SharedKernel/Enumeration.cs` — **zakres T02**, dodany tymczasowo, bo smart enums nie kompilują się bez bazy. Zgodny z D02 (`Id`, `Name`, `GetAll/FromId/FromName`, `IComparable`, równość). T02 może go rozszerzyć, nie zmieniając sygnatur.
- `tests/Warehouse.UnitTests/SharedKernel/Enums/SmartEnumTests.cs`.

## Publiczne API
```csharp
public abstract class Enumeration : IComparable<Enumeration>, IEquatable<Enumeration>
{ int Id; string Name; static IReadOnlyList<T> GetAll<T>(); static T FromId<T>(int); static T FromName<T>(string); }
public sealed class DocumentType : Enumeration { Receipt, Issue, Transfer, InternalReceipt, InternalIssue, Correction; string Code; bool RequiresApproval; bool AffectsStock; }
public sealed class MovementType : Enumeration { 11 instancji; MovementDirection? Direction; bool AffectsValuation; int? Sign; MovementDirection ResolveDirection(MovementDirection? reversed = null); }
public sealed class DocumentStatus : Enumeration { Draft, Approved, InTransit, Completed, Cancelled; bool CanTransitionTo(DocumentStatus next); }
```

## Odstępstwa i uwagi
- `MovementType.Direction` i `Sign` są **nullable** — `Reversal` ma kierunek „In/Out” zależny od storniowanego ruchu; `ResolveDirection(reversed)` zwraca przeciwny kierunek. Pozostałe 10 typów ma kierunek stały.
- `DocumentType.RequiresApproval` / `AffectsStock`: dokumentacja nie podaje wartości; wszystkie 6 typów ma `true/true` (spójne z maszyną stanów: każdy dokument przechodzi Draft → Approved i generuje ruchy). **Pytanie do architekta:** czy któryś typ ma mieć inaczej?
- `DocumentStatus.CanTransitionTo` wg diagramu D10: Draft→Approved|Cancelled, Approved→InTransit, InTransit→Completed. Ścieżki `Approved→Completed` brak (MM natychmiastowe kończy się na Approved). Ograniczenie „InTransit tylko dla MM TwoStep” egzekwuje dokument (T13), nie status.
- Identyfikatory (`Id`) smart enumów przyjęte 1..N wg kolejności w tabelach D05 (dla `DocumentType` zgodnie z tabelą). Przy persystencji (T17+) konwersja po `Id` lub `Name` — do decyzji.
- `.editorconfig`: wyłączono CA1716 (namespace `Shared` narzucony konwencjami) i CA1720 dla `PickingLineStatus.Short` (słownictwo domenowe).
