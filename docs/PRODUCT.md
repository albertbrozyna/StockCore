# PRODUCT — StockCore / Warehouse

Skrót produktowy. Pełne źródła: [`ZALOZENIA.md`](ZALOZENIA.md) (cele, zakres, NFR) i [`DOMENA.md`](DOMENA.md) (model domenowy). W razie sprzeczności wygrywa `pakiet/pakiet/01_KONWENCJE_PROJEKTU.md`.

## Czym jest
Zaawansowany system magazynowy (WMS-lite), multi-tenant, jako projekt portfolio. Fokus na API (brak frontendu w v1).

## Zasada nadrzędna
Niezmienny dziennik ruchów (`StockMovement`) jest źródłem prawdy; stan (`StockLevel`) to projekcja aktualizowana w tej samej transakcji. Korekta = nowy dokument/ruch, nigdy edycja historii.

## Funkcje (zakres)
- **Dane podstawowe:** katalog SKU (jednostki, przeliczniki, EAN, śledzenie partii/seriali), magazyny i lokalizacje, kontrahenci.
- **Dokumenty:** PZ, WZ, MM, PW, RW, KOR (+ INW generujące PW/RW).
- **Inventory:** ledger, stany, partie, numery seryjne, rezerwacje (miękkie/twarde), wycena (średnia ważona / FIFO).
- **Realizacja:** picking (FIFO/FEFO/LIFO/wskazana partia), putaway.
- **Inwentaryzacja:** spis, różnice, dokumenty korygujące.
- **Alerty:** minimalny stan, zbliżająca się data ważności, rozjazdy inwentaryzacyjne.
- **Raporty:** stan na dzień, obroty, rotacja, ABC, historia towaru (późno).

## Poza zakresem
Pełna księgowość, integracje ze sprzętem (skanery/wózki), frontend w v1.

## Kryteria sukcesu
Patrz `ZALOZENIA.md` §11 (docker compose, CI/CD, testy współbieżności, testy architektury, trace end-to-end).
