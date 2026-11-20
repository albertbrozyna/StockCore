using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Infrastructure.Persistence.Conventions;

/// <summary>A DbContext that exposes the tenant for global query filters.</summary>
public interface ITenantFilterSource
{
    TenantId CurrentTenantId { get; }
}
