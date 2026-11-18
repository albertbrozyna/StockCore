using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>
/// Entity with audit columns and a concurrency token. Values are stamped by the persistence interceptor (T17)
/// through the internal <c>Stamp*</c> methods; the entity never sets them itself.
/// </summary>
public abstract class AuditableEntity<TId> : Entity<TId>, IAuditable, IVersioned
    where TId : notnull
{
    protected AuditableEntity()
    {
    }

    protected AuditableEntity(TId id)
        : base(id)
    {
    }

    public DateTimeOffset CreatedAt { get; private set; }

    public UserId? CreatedBy { get; private set; }

    public DateTimeOffset? ModifiedAt { get; private set; }

    public UserId? ModifiedBy { get; private set; }

    public uint Version { get; private set; }

    /// <summary>Infrastructure only: stamps creation audit data when the entity is added.</summary>
    internal void StampCreated(DateTimeOffset at, UserId? by)
    {
        CreatedAt = at;
        CreatedBy = by;
    }

    /// <summary>Infrastructure only: stamps modification audit data when the entity is modified.</summary>
    internal void StampModified(DateTimeOffset at, UserId? by)
    {
        ModifiedAt = at;
        ModifiedBy = by;
    }
}
