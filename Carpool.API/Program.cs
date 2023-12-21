using API.Middleware;
using Carpool.Application.Services;
using Carpool.Domain.Interfaces;
using Carpool.Infrastructure.Context;
using Carpool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Carpool.Infrastructure.DependancyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

// Intégration de la couche d'infrastructure
builder.Services.AddDbContext<CarpoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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
app.MapControllers();
app.Run();
