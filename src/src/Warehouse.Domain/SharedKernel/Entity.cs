using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Domain.SharedKernel;

/// <summary>Base entity: identity-based equality and domain event collection.</summary>
public abstract class Entity<TId> : IEntity<TId>, IHasDomainEvents, IEquatable<Entity<TId>>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>For EF Core.</summary>
    protected Entity()
    {
        Id = default!;
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public TId Id { get; protected init; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => left is null ? right is null : left.Equals(right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);

    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UnproxiedType(this) == UnproxiedType(other)
            && !IsTransient
            && !other.IsTransient
            && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    public override int GetHashCode() => HashCode.Combine(UnproxiedType(this), Id);

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(Guard.AgainstNull(domainEvent));

    private static Type UnproxiedType(object entity)
    {
        var type = entity.GetType();
        return type.Namespace == "Castle.Proxies" && type.BaseType is { } baseType ? baseType : type;
    }

    private bool IsTransient => EqualityComparer<TId>.Default.Equals(Id, default!);
}
