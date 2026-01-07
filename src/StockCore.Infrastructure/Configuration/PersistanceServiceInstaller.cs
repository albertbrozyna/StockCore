using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Application.Common.Interfaces;
using StockCore.Infrastructure.Persistence.Context;
using StockCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Configuration
{
    public class PersistanceServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<StockDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<StockDbContext>());

        }
    }
}
