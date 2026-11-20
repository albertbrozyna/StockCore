namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Inclusive date range with <c>From &lt;= To</c>.</summary>
public sealed record DateRange
{
    private DateRange(DateOnly from, DateOnly to)
    {
        From = from;
        To = to;
    }

    public DateOnly From { get; }

    public DateOnly To { get; }

    public static DateRange Create(DateOnly from, DateOnly to) =>
        from <= to
            ? new DateRange(from, to)
            : throw ValueObjectErrors.Invalid("Common.InvalidDateRange", "'From' must not be later than 'To'.");
}
