using Carpool.Application.Interfaces;
using Carpool.Application.Services;
using Carpool.Domain.Interfaces;
using Carpool.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carpool.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPasswordHasherService, BCryptPasswordHasherService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ITokenManagerService, TokenManagerService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService>(provider =>
            {
                var secretKey = configuration["Jwt:SecretKey"];
                var audience = configuration["Jwt:Audience"];
                var issuer = configuration["Jwt:Issuer"];
                return new JwtService(secretKey, audience, issuer);
            });

            return services;
        }
    }
}
