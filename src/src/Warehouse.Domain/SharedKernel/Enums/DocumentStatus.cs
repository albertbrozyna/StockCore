namespace Warehouse.Domain.SharedKernel.Enums;

public sealed class DocumentStatus : Enumeration
{
    public static readonly DocumentStatus Draft = new(1, nameof(Draft));
    public static readonly DocumentStatus Approved = new(2, nameof(Approved));
    public static readonly DocumentStatus InTransit = new(3, nameof(InTransit));
    public static readonly DocumentStatus Completed = new(4, nameof(Completed));
    public static readonly DocumentStatus Cancelled = new(5, nameof(Cancelled));

    private DocumentStatus(int id, string name)
        : base(id, name)
    {
    }

    /// <summary>
    /// Draft → Approved | Cancelled; Approved → InTransit (two-step transfer only, enforced by the document);
    /// InTransit → Completed. Approved is otherwise terminal and changed only by a correction (KOR).
    /// </summary>
    public bool CanTransitionTo(DocumentStatus next)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (this == Draft)
        {
            return next == Approved || next == Cancelled;
        }

        if (this == Approved)
        {
            return next == InTransit;
        }

        return this == InTransit && next == Completed;
    }
}
