namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Shelf life of a lot-tracked SKU and the minimum remaining days required on receipt.</summary>
public sealed record ShelfLife
{
    private ShelfLife(int totalDays, int minRemainingDaysOnReceipt)
    {
        TotalDays = totalDays;
        MinRemainingDaysOnReceipt = minRemainingDaysOnReceipt;
    }

    public int TotalDays { get; }

    public int MinRemainingDaysOnReceipt { get; }

    public static ShelfLife Create(int totalDays, int minRemainingDaysOnReceipt)
    {
        if (totalDays < 1)
        {
            throw ValueObjectErrors.Invalid("Common.InvalidShelfLife", "TotalDays must be positive.");
        }

        return new ShelfLife(totalDays, Guard.AgainstOutOfRange(minRemainingDaysOnReceipt, 0, totalDays));
    }
}
