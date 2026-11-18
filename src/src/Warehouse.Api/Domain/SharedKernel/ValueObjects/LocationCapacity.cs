namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Optional capacity limits of a location; <c>null</c> means unlimited. Checked by <c>ILocationCapacityPolicy</c>.</summary>
public sealed record LocationCapacity
{
    private LocationCapacity(decimal? maxWeightKg, decimal? maxVolumeM3, decimal? maxUnits, int? maxPallets)
    {
        MaxWeightKg = maxWeightKg;
        MaxVolumeM3 = maxVolumeM3;
        MaxUnits = maxUnits;
        MaxPallets = maxPallets;
    }

    public decimal? MaxWeightKg { get; }

    public decimal? MaxVolumeM3 { get; }

    public decimal? MaxUnits { get; }

    public int? MaxPallets { get; }

    public static LocationCapacity Unlimited { get; } = new(null, null, null, null);

    public static LocationCapacity Create(decimal? maxWeightKg, decimal? maxVolumeM3, decimal? maxUnits, int? maxPallets) =>
        new(
            maxWeightKg is { } w ? Guard.AgainstNegative(w, nameof(maxWeightKg)) : null,
            maxVolumeM3 is { } v ? Guard.AgainstNegative(v, nameof(maxVolumeM3)) : null,
            maxUnits is { } u ? Guard.AgainstNegative(u, nameof(maxUnits)) : null,
            maxPallets is { } p ? Guard.AgainstNegative(p, nameof(maxPallets)) : null);
}
