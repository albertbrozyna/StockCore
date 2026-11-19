# ROADMAP — StockCore / Warehouse

Źródło planu zadań: [`PLAN_PRAC.md`](PLAN_PRAC.md) (T00–T22, fale 0–9; statusy tam). Kolejność faz produktu: `ZALOZENIA.md` §9. Po T22 następują slice'y `Features/…`, potem Azure/Terraform/CI-CD. Nowe funkcje spoza planu — tylko po uzgodnieniu.

## Zrobione
- **T00** szkielet rozwiązania (build zielony po poprawce audytu NuGet)
- **T01** strongly-typed ID + konwertery EF/JSON → [`raporty/RAPORT_T01.md`](raporty/RAPORT_T01.md)
- **T02** SharedKernel — klasy bazowe i abstrakcje → [`raporty/RAPORT_T02.md`](raporty/RAPORT_T02.md)
- **T17** infrastruktura: DbContext, konwencje, interceptory, outbox → [`raporty/RAPORT_T17.md`](raporty/RAPORT_T17.md)
- **T06** zdarzenia domenowe → [`raporty/RAPORT_T06.md`](raporty/RAPORT_T06.md)
- **T05** value objecty domenowe → [`raporty/RAPORT_T05.md`](raporty/RAPORT_T05.md)
- **T07** abstrakcje aplikacyjne + dispatcher CQRS → [`raporty/RAPORT_T07.md`](raporty/RAPORT_T07.md)
- **T04** value objecty ogólne → [`raporty/RAPORT_T04.md`](raporty/RAPORT_T04.md)
- **T03** enumy i smart enums → [`raporty/RAPORT_T03.md`](raporty/RAPORT_T03.md)

## Następne (kolejność z PLAN_PRAC)
1. Agregaty domenowe **T08–T14** (fala 5, do 7 równolegle), potem T15–T16 i EF T18–T20

## Znane otwarte pytania
Zob. sekcje „Odstępstwa i uwagi” w raportach (m.in. wartości `RequiresApproval`/`AffectsStock`, ArchUnitNET vs NetArchTest).
