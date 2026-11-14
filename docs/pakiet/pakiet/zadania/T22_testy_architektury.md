# T22 — Testy architektury

**Fala:** 9 · **Zależy od:** T00, T08 · **Szacunek wyjścia:** ~5k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D11_niezmienniki.md`
- RAPORT_T00
- **Raporty** zależności: `RAPORT_T00.md`, `RAPORT_T08.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T22 — Testy architektury.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- tests/Warehouse.ArchitectureTests/DomainRulesTests.cs
- tests/Warehouse.ArchitectureTests/ContextBoundariesTests.cs
- tests/Warehouse.ArchitectureTests/NamingAndStyleTests.cs

## Zakres i reguły szczegółowe

- Domain nie zależy od Infrastructure, EF Core, ASP.NET ani Application
- Granice kontekstów wg macierzy z KONWENCJE §3 (NetArchTest)
- Encje mają prywatny bezparametrowy konstruktor, brak publicznych setterów, klasy zdarzeń to sealed record dziedziczący po DomainEvent, wszystkie ID to readonly record struct
- Testy mają przechodzić już na obecnym kodzie; jeśli któryś pada z powodu błędu w kodzie — opisz w raporcie, nie poprawiaj kodu

## Poza zakresem

- Naprawy kodu

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T22.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~5k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T22b`).
