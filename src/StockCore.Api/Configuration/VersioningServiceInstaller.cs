using Asp.Versioning;
using StockCore.Shared.Interfaces;

namespace StockCore.Api.Configuration
{
    public class VersioningServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
                // Return to users a api available api version in headers
                options.ReportApiVersions = true;
                // Use default version when it is unspecified
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();

            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // Format: v1
                options.SubstituteApiVersionInUrl = true; // Podmienia {version} w URL Swaggera na konkretny numer
            });
        }
    }
}
