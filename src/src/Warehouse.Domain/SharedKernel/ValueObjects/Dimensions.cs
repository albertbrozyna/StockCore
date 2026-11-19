using Warehouse.Domain.SharedKernel.Enums;

namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Length, width and height (non-negative) in <see cref="Unit"/>.</summary>
public sealed record Dimensions
{
    private Dimensions(decimal length, decimal width, decimal height, LengthUnit unit)
    {
        Length = length;
        Width = width;
        Height = height;
        Unit = unit;
    }

    public decimal Length { get; }

    public decimal Width { get; }

    public decimal Height { get; }

    public LengthUnit Unit { get; }

    /// <summary>Volume in cubic <see cref="Unit"/>s.</summary>
    public decimal Volume => Length * Width * Height;

    public static Dimensions Create(decimal length, decimal width, decimal height, LengthUnit unit) =>
        new(Guard.AgainstNegative(length), Guard.AgainstNegative(width), Guard.AgainstNegative(height), unit);
}
