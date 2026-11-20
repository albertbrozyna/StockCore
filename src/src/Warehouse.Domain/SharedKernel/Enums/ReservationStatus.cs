namespace Warehouse.Domain.SharedKernel.Enums;

public enum ReservationStatus
{
    Active,
    PartiallyFulfilled,
    Fulfilled,
    Released,
    Expired,
    Cancelled,
}
