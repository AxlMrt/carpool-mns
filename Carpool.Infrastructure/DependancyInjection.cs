using Carpool.Infrastructure.Context;
using Carpool.Infrastructure.Interfaces;
using Carpool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carpool.Infrastructure.DependancyInjection
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarpoolDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CarpoolDbContext).Assembly.FullName)));

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
