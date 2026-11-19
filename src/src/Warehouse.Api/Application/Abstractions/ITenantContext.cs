using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Application.Abstractions;

/// <summary>Tenant of the current request (from a JWT claim or gateway header). Implemented by the API layer.</summary>
public interface ITenantContext
{
    TenantId TenantId { get; }

    bool IsResolved { get; }
}
