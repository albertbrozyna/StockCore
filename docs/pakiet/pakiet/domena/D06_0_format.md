<!-- Fragment DOMENA.md (D06_0_format). Pełny kontekst: ref/DOMENA.md -->

## 6. Bounded contexts — klasy domenowe

Format: **klasa** · baza · rola → tabela właściwości → metody domenowe → niezmienniki.
`PK` = `Id`; wszystkie klasy poza `Tenant` mają `TenantId` (dziedziczone — pomijane w tabelach).
