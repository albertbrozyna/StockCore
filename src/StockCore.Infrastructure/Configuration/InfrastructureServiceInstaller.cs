using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockCore.Application.Common.Interfaces;
using StockCore.Application.Common.Interfaces.Authentication;
using StockCore.Infrastructure.Services;
using StockCore.Infrastructure.Services.Identity;
using StockCore.Infrastructure.Services.Identity.Login;
using StockCore.Infrastructure.Services.Identity.Register;
using StockCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Configuration
{
    public class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<ILoginUserService, LoginUserService>();
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
