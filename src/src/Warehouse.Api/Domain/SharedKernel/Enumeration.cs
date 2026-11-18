using System.Reflection;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Base class for smart enums (enumerations with behaviour). Instances are public static readonly fields.</summary>
public abstract class Enumeration : IComparable<Enumeration>, IEquatable<Enumeration>
{
    protected Enumeration(int id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    public static IReadOnlyList<T> GetAll<T>()
        where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => (T)f.GetValue(null)!)
            .OrderBy(e => e.Id)
            .ToList();

    public static T FromId<T>(int id)
        where T : Enumeration =>
        GetAll<T>().FirstOrDefault(e => e.Id == id)
            ?? throw new ArgumentException($"'{id}' is not a valid id for {typeof(T).Name}.", nameof(id));

    public static T FromName<T>(string name)
        where T : Enumeration =>
        GetAll<T>().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"'{name}' is not a valid name for {typeof(T).Name}.", nameof(name));

    public static bool operator ==(Enumeration? left, Enumeration? right) => Equals(left, right);

    public static bool operator !=(Enumeration? left, Enumeration? right) => !Equals(left, right);

    public static bool operator <(Enumeration? left, Enumeration? right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator >(Enumeration? left, Enumeration? right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator <=(Enumeration? left, Enumeration? right) => !(left > right);

    public static bool operator >=(Enumeration? left, Enumeration? right) => !(left < right);

    public int CompareTo(Enumeration? other) => other is null ? 1 : Id.CompareTo(other.Id);

    public bool Equals(Enumeration? other) => other is not null && GetType() == other.GetType() && Id == other.Id;

    public override bool Equals(object? obj) => Equals(obj as Enumeration);

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public override string ToString() => Name;
}
