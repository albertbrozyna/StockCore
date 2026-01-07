using StockCore.Shared.Interfaces;

namespace StockCore.Api.Configuration
{
    public class CorsServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Frontend", builder =>
                {
                    builder.WithOrigins("https://localhost:3000");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });
        }
    }
}
