using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Warehouse.Domain.SharedKernel.Abstractions;

namespace Warehouse.Infrastructure.Persistence.Conventions;

public static class ModelConventionsExtensions
{
    private const string ConfigurationsSegment = "Configurations";

    /// <summary>
    /// Assigns the schema of every entity configured by an <c>IEntityTypeConfiguration&lt;T&gt;</c> from the folder
    /// (namespace segment after <c>Configurations</c>), lower-cased: <c>Configurations.Inventory</c> → schema <c>inventory</c>.
    /// Entities whose configuration already set a schema are left alone.
    /// </summary>
    public static ModelBuilder ApplyConfigurationSchemas(this ModelBuilder modelBuilder, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (var configType in assembly.GetTypes().Where(t => t is { IsAbstract: false, IsClass: true }))
        {
            var schema = SchemaFromNamespace(configType.Namespace);
            if (schema is null)
            {
                continue;
            }

            foreach (var iface in configType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            {
                var entityType = modelBuilder.Model.FindEntityType(iface.GetGenericArguments()[0]);
                if (entityType is { BaseType: null } && entityType.GetSchema() is null)
                {
                    entityType.SetSchema(schema);
                }
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// Domain-wide mapping rules: <c>DomainEvents</c> is never persisted; <see cref="IVersioned"/> entities use
    /// <c>Version</c> as a row version (PostgreSQL <c>xmin</c>).
    /// </summary>
    public static ModelBuilder ApplyDomainConventions(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()).ToList())
        {
            var clr = entityType.ClrType;
            if (typeof(IHasDomainEvents).IsAssignableFrom(clr))
            {
                modelBuilder.Entity(clr).Ignore(nameof(IHasDomainEvents.DomainEvents));
            }

            if (typeof(IVersioned).IsAssignableFrom(clr) && entityType.BaseType is null)
            {
                modelBuilder.Entity(clr).Property(nameof(IVersioned.Version)).IsRowVersion();
            }
        }

        return modelBuilder;
    }

    private static string? SchemaFromNamespace(string? ns)
    {
        if (ns is null)
        {
            return null;
        }

        var segments = ns.Split('.');
        var index = Array.IndexOf(segments, ConfigurationsSegment);
        return index >= 0 && index + 1 < segments.Length ? segments[index + 1].ToLowerInvariant() : null;
    }
}
