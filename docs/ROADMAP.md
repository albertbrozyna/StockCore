# ROADMAP — StockCore / Warehouse

Źródło planu zadań: [`PLAN_PRAC.md`](PLAN_PRAC.md) (T00–T22, fale 0–9; statusy tam). Kolejność faz produktu: `ZALOZENIA.md` §9. Po T22 następują slice'y `Features/…`, potem Azure/Terraform/CI-CD. Nowe funkcje spoza planu — tylko po uzgodnieniu.

## Zrobione
- **T00** szkielet rozwiązania (build zielony po poprawce audytu NuGet)
- **T01** strongly-typed ID + konwertery EF/JSON → [`raporty/RAPORT_T01.md`](raporty/RAPORT_T01.md)
- **T03** enumy i smart enums → [`raporty/RAPORT_T03.md`](raporty/RAPORT_T03.md)

## Następne (kolejność z PLAN_PRAC)
1. **T02** SharedKernel — klasy bazowe (`Entity`, `Result`, `Error`, `Guard`, …) — odblokowuje falę 3
2. **T04** value objecty ogólne · **T07** abstrakcje aplikacyjne (równolegle)
3. **T05, T06, T17** (równolegle), potem agregaty T08–T14

## Znane otwarte pytania
Zob. sekcje „Odstępstwa i uwagi” w raportach (m.in. wartości `RequiresApproval`/`AffectsStock`, ArchUnitNET vs NetArchTest).
