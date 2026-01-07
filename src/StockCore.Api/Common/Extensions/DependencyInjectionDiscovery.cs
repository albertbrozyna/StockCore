namespace StockCore.Api.Common.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StockCore.Shared.Interfaces;
    using System.Reflection;

    public static class DependencyInjectionDiscovery
    {
        public static IServiceCollection InstallServicesInAssemblies(
            this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            // Szukamy wszystkich klas, które implementują IServiceInstaller, 
            // nie są interfejsami i nie są klasami abstrakcyjnymi
            var installers = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(t => typeof(IServiceInstaller).IsAssignableFrom(t) &&
                            !t.IsInterface &&
                            !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>()
                .ToList();

            // Uruchamiamy każdy znaleziony instalator
            foreach (var installer in installers)
            {
                installer.Install(services, configuration);
            }

            return services;
        }
    }
}
