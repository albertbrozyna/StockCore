using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Infrastructure.Persistence;

namespace StockCore.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task UseDatabaseInitializer(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            await initializer.InitializeAsync();
        }
    }
}