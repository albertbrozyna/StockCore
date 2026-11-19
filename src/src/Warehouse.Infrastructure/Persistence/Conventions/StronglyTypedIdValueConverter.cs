using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Infrastructure.Persistence.Conventions;

/// <summary>Maps a Guid-based strongly-typed ID to its underlying Guid column.</summary>
public sealed class StronglyTypedIdValueConverter<TId> : ValueConverter<TId, Guid>
    where TId : struct, IStronglyTypedId<Guid>
{
    public StronglyTypedIdValueConverter()
        : base(ToProvider(), FromProvider())
    {
    }

    private static Expression<Func<TId, Guid>> ToProvider()
    {
        var id = Expression.Parameter(typeof(TId), "id");
        return Expression.Lambda<Func<TId, Guid>>(Expression.Property(id, nameof(IStronglyTypedId<Guid>.Value)), id);
    }

    private static Expression<Func<Guid, TId>> FromProvider()
    {
        var value = Expression.Parameter(typeof(Guid), "value");
        var ctor = typeof(TId).GetConstructor([typeof(Guid)])
            ?? throw new InvalidOperationException($"{typeof(TId).Name} must have a public constructor taking a Guid.");
        return Expression.Lambda<Func<Guid, TId>>(Expression.New(ctor, value), value);
    }
}
