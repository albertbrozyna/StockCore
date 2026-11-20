using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Abstractions.Messaging;
using Warehouse.Application.Behaviors;

namespace Warehouse.Application;

public static class DependencyInjection
{
    /// <summary>Registers the dispatcher, pipeline behaviors, and scans this assembly for handlers and validators.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IDispatcher, Dispatcher>();

        // Order = pipeline order, outermost first.
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TenantGuardBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services.AddHandlersFromAssembly(typeof(DependencyInjection).Assembly);
    }

    /// <summary>Registers every concrete command/query handler and FluentValidation validator found in <paramref name="assembly"/>.</summary>
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (var type in assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }))
        {
            foreach (var service in type.GetInterfaces().Where(IsHandlerOrValidator))
            {
                services.AddScoped(service, type);
            }
        }

        return services;
    }

    private static bool IsHandlerOrValidator(Type type) =>
        type.IsGenericType
        && type.GetGenericTypeDefinition() is var d
        && (d == typeof(ICommandHandler<,>) || d == typeof(IQueryHandler<,>) || d == typeof(IValidator<>));
}
