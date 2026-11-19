namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>How document numbers are generated for a tenant.</summary>
public sealed record DocumentNumberingPolicy(bool PerWarehouse, bool ResetYearly, int SequencePadding, string? PrefixOverride)
{
    public static DocumentNumberingPolicy Default { get; } = new(PerWarehouse: true, ResetYearly: true, SequencePadding: 6, PrefixOverride: null);
}
