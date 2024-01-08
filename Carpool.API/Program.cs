using API.Middleware;
using Carpool.Application.Services;
using Carpool.Infrastructure.Context;
using Carpool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Carpool.Infrastructure.DependancyInjection;
using Carpool.Application.Interfaces;
using Carpool.Infrastructure.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Carpool.Application;
using Microsoft.OpenApi.Models;
using Carpool.Infrastructure;
using Carpool.Domain.Interfaces;
using System.Text.Json.Serialization;
using Carpool.Domain.Services;
using Carpool.ChatHub;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCors();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Carpool", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<CarpoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IChatHubService, ChatHubService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPasswordHasherService, BCryptPasswordHasherService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITokenManagerService, TokenManagerService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJwtService>(provider =>
{
    var secretKey = builder.Configuration["Jwt:SecretKey"];
    var audience = builder.Configuration["Jwt:Audience"];
    var issuer = builder.Configuration["Jwt:Issuer"];
    return new JwtService(secretKey, audience, issuer);
});

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<CarpoolDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Connexion à la base de données établie avec succès !");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur de connexion à la base de données : {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://127.0.0.1:5500");
});
app.MapControllers();
app.MapHub<ChatHubService>("/chat-hub");
app.Run();
