<!-- Fragment DOMENA.md (D11_niezmienniki). Pełny kontekst: ref/DOMENA.md -->

## 11. Najważniejsze niezmienniki systemowe (checklista dla implementatora)

1. Każdy zapis do `StockLevel` ma **odpowiadający wpis w `StockMovement`** w tej samej transakcji; zero wyjątków.
2. `StockMovement` jest append-only (blokada w EF + trigger Postgres).
3. `StockLevel.OnHand == Σ StockMovement.QuantityDelta` dla tego samego klucza (test spójności uruchamiany cyklicznie jako job).
4. Zatwierdzony dokument jest niemutowalny; korekta = nowy dokument `KOR`.
5. Stan nie schodzi poniżej zera (poza `AllowNegativeStock`) — egzekwowane w domenie **i** `CHECK` w bazie.
6. Rezerwacja twarda nigdy nie przekracza `Available`.
7. Partie `Blocked` / `Expired` / `Quarantine` nigdy nie są alokowane do wydań.
8. Numer seryjny istnieje w jednej lokalizacji naraz; seryjny wydany nie wraca do stanu bez `Return`.
9. Wszystkie dane tenanta izolowane: filtr globalny + guard zapisu + `TenantId` w każdym indeksie.
10. Żaden slice nie referuje innego slice'a; współdzielenie wyłącznie przez `Domain/` i `Shared/`.
