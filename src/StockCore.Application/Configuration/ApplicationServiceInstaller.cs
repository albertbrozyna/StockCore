using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Application.Common.Behaviors;
using StockCore.Application.Common.Interfaces;
using StockCore.Application.Common.Settings;
using StockCore.Shared.Interfaces;
using System.Reflection;

namespace StockCore.Application.Configuration
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IAppDbContext).Assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            }

                );

            // Filestorage settings
            services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));


            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
        }
    }
}
