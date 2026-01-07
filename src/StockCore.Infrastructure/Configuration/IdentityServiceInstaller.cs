using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Application.Common.Interfaces.Authentication;
using StockCore.Domain.Entities;
using StockCore.Infrastructure.Persistence.Context;
using StockCore.Infrastructure.Services;
using StockCore.Shared.Interfaces;

namespace StockCore.Infrastructure.Configuration
{
    public class IdentityServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
          .AddRoles<IdentityRole<Guid>>()
           .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
           .AddUserManager<UserManager<User>>()
           .AddSignInManager<SignInManager<User>>()
           .AddEntityFrameworkStores<StockDbContext>();

            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
