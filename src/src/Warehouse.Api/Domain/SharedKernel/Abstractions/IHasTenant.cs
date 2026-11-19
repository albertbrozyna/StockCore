using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel.Abstractions;

/// <summary>An object that belongs to a tenant.</summary>
public interface IHasTenant
{
    TenantId TenantId { get; }
}
