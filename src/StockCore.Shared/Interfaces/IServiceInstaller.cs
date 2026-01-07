using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockCore.Shared.Interfaces
{
    public interface IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration);
    }
}
