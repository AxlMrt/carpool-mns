using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carpool.API.Configurations
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarpoolDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
