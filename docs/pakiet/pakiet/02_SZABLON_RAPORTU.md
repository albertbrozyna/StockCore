# RAPORT_Txx — szablon (wypełnij na końcu zadania)

```markdown
# RAPORT_T<nr> — <tytuł zadania>
Status: DONE | PARTIAL
Zbudowano: tak/nie (dotnet build) · Testy: przeszły/nie dotyczy

## Pliki
- ścieżka/Plik.cs — jedno zdanie

## Publiczne API (tylko sygnatury — to jest kontrakt dla następnych zadań)
```csharp
// namespace ...
public sealed class Foo : TenantEntity<FooId> { public static Foo Create(...); public void Bar(...); ... }
```

## Odstępstwa i uwagi
- co zrobiono inaczej niż w domena/*.md lub KONWENCJE i dlaczego
- pytania do architekta

## Niewykonane (TODO dla kontynuacji)
- ...

## Wskazówki dla zależnych zadań
- ...
```

Zasady: raport ≤ ~250 linii; sygnatury publiczne wszystkich nowych typów (bez ciał metod); nie wklejaj całego kodu.
