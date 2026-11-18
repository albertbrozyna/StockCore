using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>
/// Tenant entity deactivated logically. <see cref="Delete"/> only raises the flag; <see cref="DeletedAt"/> and
/// <see cref="DeletedBy"/> are stamped by the persistence interceptor via the internal <see cref="StampDeleted"/>.
/// </summary>
public abstract class SoftDeletableTenantEntity<TId> : TenantEntity<TId>, ISoftDeletable
    where TId : notnull
{
    protected SoftDeletableTenantEntity()
    {
    }

    protected SoftDeletableTenantEntity(TId id)
        : base(id)
    {
    }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public UserId? DeletedBy { get; private set; }

    public void Delete() => IsDeleted = true;

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }

    /// <summary>Infrastructure only: stamps deletion audit data for a deleted entity.</summary>
    internal void StampDeleted(DateTimeOffset at, UserId? by)
    {
        DeletedAt = at;
        DeletedBy = by;
    }
}
