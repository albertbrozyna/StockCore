namespace Warehouse.Domain.SharedKernel.Enums;

public enum StockTakeStatus
{
    Planned,
    InProgress,
    CountingClosed,
    UnderReview,
    Approved,
    Cancelled,
}
