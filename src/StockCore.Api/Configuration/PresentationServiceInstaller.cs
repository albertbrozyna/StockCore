using StockCore.Api.Infrastructure;
using StockCore.Application.Common.Interfaces;
using StockCore.Shared.Interfaces;

namespace StockCore.Api.Configuration
{
    public class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserDetails, CurrentUserDetails>();
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(configure => {
                configure.CustomizeProblemDetails = options => {
                    options.ProblemDetails.Extensions.TryAdd("requestId", options.HttpContext.TraceIdentifier);
                };
            });
        }
    }
}
