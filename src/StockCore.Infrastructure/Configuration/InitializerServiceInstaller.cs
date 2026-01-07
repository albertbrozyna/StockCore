using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Infrastructure.Persistence;
using StockCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Configuration
{
    public class InitializerServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DatabaseInitializer>();
        }
    }
}
