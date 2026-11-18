using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

public static class QueryFilterExtensions
{
    /// <summary>
    /// Adds a global query filter to every root entity: <c>TenantId == tenant</c> for <see cref="IHasTenant"/> and
    /// <c>!IsDeleted</c> for <see cref="ISoftDeletable"/>. An unresolved tenant matches nothing (fail closed).
    /// </summary>
    /// <remarks>
    /// The tenant is read from <paramref name="context"/> (a <see cref="DbContext"/> member), not from a captured service:
    /// EF caches the model, and only DbContext members are re-evaluated for every context instance.
    /// </remarks>
    public static ModelBuilder ApplyTenantAndSoftDeleteFilters<TContext>(this ModelBuilder modelBuilder, TContext context)
        where TContext : DbContext, ITenantFilterSource
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(context);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => e.BaseType is null && !e.IsOwned() && !e.IsKeyless).ToList())
        {
            var clr = entityType.ClrType;
            var hasTenant = typeof(IHasTenant).IsAssignableFrom(clr);
            var softDeletable = typeof(ISoftDeletable).IsAssignableFrom(clr);
            if (!hasTenant && !softDeletable)
            {
                continue;
            }

            var parameter = Expression.Parameter(clr, "e");
            Expression? body = null;
            if (hasTenant)
            {
                var current = Expression.Property(Expression.Constant(context, typeof(TContext)), typeof(TContext).GetProperty(nameof(ITenantFilterSource.CurrentTenantId))!);
                body = Expression.Equal(Expression.Property(parameter, nameof(IHasTenant.TenantId)), current);
            }

            if (softDeletable)
            {
                var notDeleted = Expression.Not(Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)));
                body = body is null ? notDeleted : Expression.AndAlso(body, notDeleted);
            }

            modelBuilder.Entity(clr).HasQueryFilter(Expression.Lambda(body!, parameter));
        }

        return modelBuilder;
    }
}
