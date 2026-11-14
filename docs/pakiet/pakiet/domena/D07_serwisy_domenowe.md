<!-- Fragment DOMENA.md (D07_serwisy_domenowe). Pełny kontekst: ref/DOMENA.md -->

## 7. Serwisy domenowe i polityki (interfejsy w `Domain/`)

Serwisy domenowe są **czyste** (bez I/O): potrzebne dane dostają jako parametry. Dostęp do bazy jest w slice'ach / `Infrastructure/`.

```csharp
// Księgowanie dokumentu w ledgerze + aktualizacja StockLevel (jedna transakcja)
public interface IStockPostingService
{
    PostingResult Post(WarehouseDocument document,
                       IReadOnlyDictionary<StockLevelKey, StockLevel> affectedLevels,
                       PostingContext context);          // zwraca StockMovement[] + zmodyfikowane StockLevel
}

public interface IStockAllocationStrategy                  // wybór, z których StockLevel/Lot wydać
{
    AllocationStrategyType Type { get; }
    AllocationResult Allocate(Quantity requested,
                              IReadOnlyCollection<AllocationCandidate> candidates,
                              AllocationContext context);
}
// implementacje: FifoAllocationStrategy, FefoAllocationStrategy, LifoAllocationStrategy, SpecificLotAllocationStrategy

public interface IValuationStrategy                        // koszt wydania
{
    ValuationMethod Method { get; }
    ValuationResult CalculateIssueCost(Quantity quantity, ValuationState state);
    void ApplyReceipt(ValuationState state, Quantity quantity, Money unitCost);
}
// implementacje: WeightedAverageValuationStrategy (SkuCostBalance), FifoValuationStrategy (CostLayer)

public interface IDocumentNumberGenerator
{
    DocumentNumber Next(DocumentType type, WarehouseCode warehouse, DateOnly date,
                        DocumentNumberSequence sequence, DocumentNumberingPolicy policy);
}

public interface IPutawayStrategy
{
    LocationId? Suggest(PutawayContext context, IReadOnlyCollection<Location> candidates);
}
// implementacje: FixedLocationPutaway, NearestEmptyPutaway, ByCategoryPutaway

public interface IStockAvailabilityPolicy                  // czy można wydać / zarezerwować
{
    Result CanIssue(StockLevel level, Quantity quantity, bool allowNegative);
    Result CanReserve(StockLevel level, Quantity quantity);
}

public interface ILocationCapacityPolicy
{
    Result CanStore(Location location, Quantity quantity, Dimensions? dimensions, Weight? weight, LocationLoad currentLoad);
}

public interface IExpiryPolicy
{
    Result ValidateOnReceipt(Sku sku, DateOnly? productionDate, DateOnly? expiryDate, DateOnly today);
    bool IsSellable(Lot lot, DateOnly today, int minRemainingDays);
}

public interface IUnitConverter
{
    Quantity ToBase(Sku sku, Quantity quantity);
    Quantity FromBase(Sku sku, decimal baseAmount, UnitCode target);
}

public interface IStockTakeVariancePolicy
{
    bool RequiresRecount(StockTakeLine line, decimal tolerancePercent);
}
```

Pomocnicze typy: `MovementDraft` (opis zamierzonego ruchu zwracany przez `BuildMovements()`), `PostingResult`, `PostingContext` (`DateTimeOffset now`, `UserId`, `Guid correlationId`, `TenantSettings`), `AllocationCandidate` (`StockLevel`, `Lot?`), `AllocationResult`, `StockLevelKey` (`LocationId`, `SkuId`, `LotId?`, `StockStatus`).
