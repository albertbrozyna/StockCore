# T06 — Zdarzenia domenowe (wszystkie)

**Fala:** 4 · **Zależy od:** T01, T02, T03 · **Szacunek wyjścia:** ~6k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D09_zdarzenia.md`
- `domena/D02_sharedkernel.md` (DomainEvent)
- **Raporty** zależności: `RAPORT_T01.md`, `RAPORT_T02.md`, `RAPORT_T03.md` (raporty, **nie** cały kod — chyba że raport każe inaczej)

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T06 — Zdarzenia domenowe (wszystkie).
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/<Kontekst>/Events/*.cs — po jednym `sealed record` na zdarzenie, dziedziczą po `DomainEvent`
- Domain/SharedKernel/Abstractions/IIntegrationEvent.cs (marker dla zdarzeń publikowanych przez outbox) + implementują je zdarzenia oznaczone ✔ w tabeli D09

## Zakres i reguły szczegółowe

- Zdarzenia używają **tylko ID, enumów i prymitywów** (decimal, string, DateTimeOffset) — bez value objectów
- Pola wg kolumny „Kluczowe pola" z D09; dodaj `TenantId` przez konstruktor bazowy
- Mapowanie zdarzenie → kontekst wg kolumny „Zgłaszane przez" (Tenancy, Catalog, Locations, Inventory, Documents, Fulfillment, Stocktaking)

## Poza zakresem

- Handlery zdarzeń
- Logika dispatchu (T17)

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T06.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~6k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T06b`).
