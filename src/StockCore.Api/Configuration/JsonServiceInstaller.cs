using Microsoft.AspNetCore.Http.Json;
using StockCore.Shared.Interfaces;

namespace StockCore.Api.Configuration
{
    public class JsonServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
            });
        }
    }
}
