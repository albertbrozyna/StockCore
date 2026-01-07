
using Microsoft.AspNetCore.Http.Timeouts;
using StockCore.Shared.Interfaces;

namespace StockCore.Api.Configuration
{
    public class TimeoutsServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddRequestTimeouts(options =>
            {
                // Ustawiamy domyślną politykę dla całej aplikacji
                options.DefaultPolicy = new RequestTimeoutPolicy
                {
                    Timeout = TimeSpan.FromSeconds(240),
                    WriteTimeoutResponse = async (context) =>
                    {
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\": \"Przekroczono czas oczekiwania (Timeout).\"}");
                    }
                };

                // Możesz też dodać nazwane polityki dla konkretnych endpointów
                options.AddPolicy("LongRunning", TimeSpan.FromMinutes(5));
            });
        }
    }
}
