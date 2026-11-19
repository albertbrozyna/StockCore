using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Application.Abstractions;

/// <summary>Tenant of the current request (from a JWT claim or gateway header). Implemented by the API layer.</summary>
public interface ITenantContext
{
    TenantId TenantId { get; }

    bool IsResolved { get; }
}
