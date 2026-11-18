using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>
/// Append-only tenant entry (ledger). Has no mutating methods; the interceptor rejects Modified/Deleted states.
/// Audit data and tenant are stamped through the internal <c>Stamp*</c>/<see cref="SetTenant"/> methods.
/// </summary>
public abstract class ImmutableTenantEntity<TId> : Entity<TId>, IHasTenant, ICreationAudited
    where TId : notnull
{
    protected ImmutableTenantEntity()
    {
    }

    protected ImmutableTenantEntity(TId id)
        : base(id)
    {
    }

    public TenantId TenantId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public UserId? CreatedBy { get; private set; }

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

    /// <summary>Infrastructure only: stamps creation audit data when the entry is added.</summary>
    internal void StampCreated(DateTimeOffset at, UserId? by)
    {
        CreatedAt = at;
        CreatedBy = by;
    }
}
