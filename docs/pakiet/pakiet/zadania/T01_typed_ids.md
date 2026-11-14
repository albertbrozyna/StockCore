# T01 — Strongly-typed ID + konwertery

**Fala:** 1 · **Zależy od:** brak · **Szacunek wyjścia:** ~6k tokenów

## Co załączyć do nowego czatu

- `01_KONWENCJE_PROJEKTU.md` i `02_SZABLON_RAPORTU.md` (zawsze)
- `domena/D03_identyfikatory.md`

## Prompt startowy (wklej jako pierwszą wiadomość)

```text
Jesteś implementatorem w projekcie **Warehouse** (.NET 10, vertical slice + DDD, multi-tenant). Wykonujesz WYŁĄCZNIE zadanie opisane poniżej. Trzymaj się załączonych plików konwencji i fragmentów domeny. Rozmawiamy po polsku; kod i identyfikatory po angielsku. Na końcu wygeneruj raport wg szablonu `02_SZABLON_RAPORTU.md`.

Zadanie: T01 — Strongly-typed ID + konwertery.
Zakres, pliki do wytworzenia i kryteria ukończenia są w pliku zadania (wklejam go poniżej).
```

## Pliki do wytworzenia

- Domain/SharedKernel/Ids/*.cs — po jednym pliku na ID (lista w D03)
- Domain/SharedKernel/Abstractions/IStronglyTypedId.cs
- Infrastructure/Persistence/Conventions/StronglyTypedIdValueConverter.cs (generyczny ValueConverter<TId,Guid>) + rejestracja wszystkich ID przez refleksję (metoda rozszerzająca na ModelConfigurationBuilder)
- Shared/Json/StronglyTypedIdJsonConverterFactory.cs (System.Text.Json, serializacja jako string GUID)

## Zakres i reguły szczegółowe

- Wszystkie ID z D03, każde wg wzorca z KONWENCJE §5 (New, Empty, IsEmpty, ToString)
- Tylko `TenantId` i `UserId` oraz `IStronglyTypedId` są potrzebne wszystkim — zrób je jako pierwsze

## Poza zakresem

- EF DbContext
- Jakiekolwiek encje

## Definition of Done

- [ ] Wszystkie pliki z listy istnieją, jeden typ publiczny na plik, namespace = ścieżka
- [ ] Zgodność z `01_KONWENCJE_PROJEKTU.md` i fragmentami `domena/*.md` (nazwy i typy właściwości 1:1)
- [ ] Kod się buduje (lub został przejrzany pod kątem kompilacji, jeśli brak `dotnet`)
- [ ] Wygenerowany `RAPORT_T01.md` (sygnatury publiczne, odstępstwa, TODO)

## Budżet tokenów

Szacunek wyjścia: ~6k tokenów. Jeśli zaczyna brakować miejsca: **dokończ bieżący plik**, nie zaczynaj następnego, zapisz raport ze statusem `PARTIAL` i listą niewykonanych plików — raport posłuży jako brief na czat kontynuacyjny (`T01b`).
