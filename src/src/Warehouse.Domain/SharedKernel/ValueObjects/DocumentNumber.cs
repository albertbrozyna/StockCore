namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Document number such as <c>PZ/WH1/2026/000123</c>. Created only by the document number generator.</summary>
public sealed record DocumentNumber
{
    private DocumentNumber(string prefix, int year, long sequence, string full)
    {
        Prefix = prefix;
        Year = year;
        Sequence = sequence;
        Full = full;
    }

    public string Prefix { get; }

    public int Year { get; }

    public long Sequence { get; }

    public string Full { get; }

    public static DocumentNumber Create(string prefix, int year, long sequence, int padding)
    {
        var p = Guard.AgainstNullOrWhiteSpace(prefix).Trim();
        Guard.AgainstOutOfRange(year, 1900, 9999);
        Guard.AgainstOutOfRange(padding, 1, 18);
        if (sequence < 1)
        {
            throw ValueObjectErrors.Invalid("Common.InvalidDocumentNumber", "Sequence must be positive.");
        }

        var full = FormattableString.Invariant($"{p}/{year}/{sequence.ToString("D" + padding, System.Globalization.CultureInfo.InvariantCulture)}");
        return new DocumentNumber(p, year, sequence, full);
    }

    public override string ToString() => Full;
}
