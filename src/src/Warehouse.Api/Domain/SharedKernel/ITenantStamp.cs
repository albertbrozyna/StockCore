using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Non-generic infrastructure hook so persistence interceptors can assign the tenant once.</summary>
internal interface ITenantStamp
{
    void SetTenant(TenantId tenantId);
}
