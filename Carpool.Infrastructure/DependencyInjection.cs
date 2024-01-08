using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Context;
using Carpool.Infrastructure.Interfaces;
using Carpool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carpool.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarpoolDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CarpoolDbContext).Assembly.FullName)));

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ITokenManagerRepository, TokenManagerRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
