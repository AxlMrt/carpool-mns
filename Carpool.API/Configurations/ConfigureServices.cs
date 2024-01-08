using Carpool.Application.DependencyInjection;
using Carpool.Infrastructure.DependencyInjection;

namespace Carpool.API.Configurations
{
    public static class ApplicationServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);
            services.AddApplication(configuration);
            services.AddSignalR();
        }
    }
}
