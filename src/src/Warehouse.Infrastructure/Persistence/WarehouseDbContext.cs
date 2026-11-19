using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Infrastructure.Persistence.Conventions;
using Warehouse.Infrastructure.Persistence.Outbox;

namespace Warehouse.Infrastructure.Persistence;

/// <summary>
/// Application DbContext. Context-specific <c>DbSet</c>s are added by T18–T20; entity configurations are discovered
/// from <c>Configurations/&lt;Folder&gt;</c> and placed in the schema named after the folder.
/// </summary>
public sealed class WarehouseDbContext(DbContextOptions<WarehouseDbContext> options, ITenantContext tenantContext) : DbContext(options), ITenantFilterSource
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    /// <summary>Tenant used by global query filters; empty (matches nothing) when the tenant is not resolved.</summary>
    public TenantId CurrentTenantId => tenantContext.IsResolved ? tenantContext.TenantId : default;

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        configurationBuilder.ConfigurePersistenceConventions();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        var assembly = typeof(WarehouseDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        modelBuilder.ApplyConfigurationSchemas(assembly);
        modelBuilder.ApplyDomainConventions();
        modelBuilder.ApplyTenantAndSoftDeleteFilters(this);
    }
}
