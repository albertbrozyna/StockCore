using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>
/// Audited entity owned by a tenant. <see cref="TenantId"/> is set once — by the interceptor from the tenant context,
/// or by a parent aggregate's factory — via the internal <see cref="SetTenant"/>; it cannot change afterwards.
/// </summary>
public abstract class TenantEntity<TId> : AuditableEntity<TId>, IHasTenant, ITenantStamp
    where TId : notnull
{
    protected TenantEntity()
    {
    }

    protected TenantEntity(TId id)
        : base(id)
    {
    }

    public TenantId TenantId { get; private set; }

    /// <summary>Sets the tenant once; re-setting the same value is a no-op, a different value throws.</summary>
    internal void SetTenant(TenantId tenantId)
    {
        Guard.AgainstDefault(tenantId);
        if (!TenantId.IsEmpty && TenantId != tenantId)
        {
            throw new DomainException(new Error("SharedKernel.TenantImmutable", "TenantId cannot be changed once set.", ErrorType.Conflict));
        }

        TenantId = tenantId;
    }

    void ITenantStamp.SetTenant(TenantId tenantId) => SetTenant(tenantId);
}
