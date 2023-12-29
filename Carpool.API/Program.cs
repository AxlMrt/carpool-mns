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
using Carpool.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

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
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasherService, BCryptPasswordHasherService>();
builder.Services.AddScoped<IJwtService>(provider =>
{
    var secretKey = builder.Configuration["Jwt:SecretKey"];
    return new JwtService(secretKey);
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173");
});

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

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
