using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

/// <summary>A DbContext that exposes the tenant for global query filters.</summary>
public interface ITenantFilterSource
{
    TenantId CurrentTenantId { get; }
}
