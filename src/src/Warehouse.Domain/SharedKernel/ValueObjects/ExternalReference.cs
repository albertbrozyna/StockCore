namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Correlation with an external system, e.g. <c>("ERP-ORDER", "ZK/12/2026")</c>.</summary>
public sealed record ExternalReference
{
    private ExternalReference(string source, string value)
    {
        Source = source;
        Value = value;
    }

    public string Source { get; }

    public string Value { get; }

    public static ExternalReference Create(string source, string value) =>
        new(Guard.AgainstNullOrWhiteSpace(source).Trim(), Guard.AgainstNullOrWhiteSpace(value).Trim());
}
