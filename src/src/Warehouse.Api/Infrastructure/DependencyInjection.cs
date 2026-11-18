using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Infrastructure.Persistence;
using Warehouse.Api.Infrastructure.Persistence.Interceptors;
using Warehouse.Api.Infrastructure.Persistence.Outbox;

namespace Warehouse.Api.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers <see cref="WarehouseDbContext"/> (PostgreSQL), its interceptors, the unit of work, the outbox publisher and the
    /// outbox processor. The host must also provide <see cref="ITenantContext"/> and <see cref="ICurrentUser"/>.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddScoped<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
        services.TryAddScoped<IOutboxMessageDispatcher, LoggingOutboxMessageDispatcher>();

        services.AddScoped<ImmutableEntityGuard>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<AuditingInterceptor>();
        services.AddScoped<TenantSaveChangesGuard>();
        services.AddScoped<OutboxInterceptor>();
        services.AddScoped<DomainEventsInterceptor>();

        services.AddDbContext<WarehouseDbContext>((sp, options) => options
            .UseNpgsql(connectionString)
            // Order matters: guards first, then stamping, then outbox (reads events) before dispatcher (clears them).
            .AddInterceptors(
                sp.GetRequiredService<ImmutableEntityGuard>(),
                sp.GetRequiredService<SoftDeleteInterceptor>(),
                sp.GetRequiredService<AuditingInterceptor>(),
                sp.GetRequiredService<TenantSaveChangesGuard>(),
                sp.GetRequiredService<OutboxInterceptor>(),
                sp.GetRequiredService<DomainEventsInterceptor>()));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IIntegrationEventPublisher, OutboxIntegrationEventPublisher>();
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
