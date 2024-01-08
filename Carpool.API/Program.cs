using API.Middleware;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Carpool.ChatHub;
using Carpool.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);
ConfigureApp(builder);

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

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    SwaggerConfiguration.ConfigureSwagger(services);
    DatabaseConfiguration.ConfigureDatabase(services, configuration);
    JwtAuthenticationConfiguration.ConfigureJwtAuthentication(services, configuration);
    ApplicationServiceConfiguration.ConfigureServices(services, configuration);

    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    services.AddCors();
}

void ConfigureApp(WebApplicationBuilder builder)
{
    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseMiddleware<ExceptionMiddleware>();

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
}
