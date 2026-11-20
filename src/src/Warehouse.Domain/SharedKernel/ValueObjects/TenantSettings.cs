using Warehouse.Domain.SharedKernel.Enums;

namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Per-tenant configuration, owned by <c>Tenant</c>.</summary>
public sealed record TenantSettings(
    bool AllowNegativeStock,
    AllocationStrategyType DefaultAllocationStrategy,
    ValuationMethod DefaultValuationMethod,
    TimeSpan? DefaultReservationTtl,
    CurrencyCode DefaultCurrency,
    bool RequireLocationOnReceipt,
    DocumentNumberingPolicy Numbering)
{
    public static TenantSettings Default { get; } = new(
        AllowNegativeStock: false,
        DefaultAllocationStrategy: AllocationStrategyType.Fifo,
        DefaultValuationMethod: ValuationMethod.WeightedAverage,
        DefaultReservationTtl: null,
        DefaultCurrency: CurrencyCode.Pln,
        RequireLocationOnReceipt: false,
        Numbering: DocumentNumberingPolicy.Default);
}
