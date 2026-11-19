namespace Warehouse.Domain.SharedKernel.Enums;

public sealed class MovementType : Enumeration
{
    public static readonly MovementType ReceiptExternal = new(1, nameof(ReceiptExternal), MovementDirection.In, affectsValuation: true);
    public static readonly MovementType IssueExternal = new(2, nameof(IssueExternal), MovementDirection.Out, affectsValuation: true);
    public static readonly MovementType TransferOut = new(3, nameof(TransferOut), MovementDirection.Out, affectsValuation: false);
    public static readonly MovementType TransferIn = new(4, nameof(TransferIn), MovementDirection.In, affectsValuation: false);
    public static readonly MovementType ReceiptInternal = new(5, nameof(ReceiptInternal), MovementDirection.In, affectsValuation: true);
    public static readonly MovementType IssueInternal = new(6, nameof(IssueInternal), MovementDirection.Out, affectsValuation: true);
    public static readonly MovementType StocktakeSurplus = new(7, nameof(StocktakeSurplus), MovementDirection.In, affectsValuation: true);
    public static readonly MovementType StocktakeShortage = new(8, nameof(StocktakeShortage), MovementDirection.Out, affectsValuation: true);
    public static readonly MovementType CorrectionIncrease = new(9, nameof(CorrectionIncrease), MovementDirection.In, affectsValuation: true);
    public static readonly MovementType CorrectionDecrease = new(10, nameof(CorrectionDecrease), MovementDirection.Out, affectsValuation: true);

    /// <summary>Storno of an earlier movement; direction is opposite to the reversed one (see <see cref="ResolveDirection"/>).</summary>
    public static readonly MovementType Reversal = new(11, nameof(Reversal), direction: null, affectsValuation: true);

    private MovementType(int id, string name, MovementDirection? direction, bool affectsValuation)
        : base(id, name)
    {
        Direction = direction;
        AffectsValuation = affectsValuation;
    }

    /// <summary>Fixed direction, or null for <see cref="Reversal"/> (depends on the reversed movement).</summary>
    public MovementDirection? Direction { get; }

    public bool AffectsValuation { get; }

    /// <summary>+1 for In, -1 for Out, null when the direction is not fixed.</summary>
    public int? Sign => Direction switch
    {
        MovementDirection.In => 1,
        MovementDirection.Out => -1,
        _ => null,
    };

    /// <summary>Returns the effective direction; for <see cref="Reversal"/> it is the opposite of <paramref name="reversed"/>.</summary>
    public MovementDirection ResolveDirection(MovementDirection? reversed = null)
    {
        if (Direction is { } fixedDirection)
        {
            return fixedDirection;
        }

        return reversed switch
        {
            MovementDirection.In => MovementDirection.Out,
            MovementDirection.Out => MovementDirection.In,
            _ => throw new InvalidOperationException($"{Name} requires the direction of the reversed movement."),
        };
    }
}
