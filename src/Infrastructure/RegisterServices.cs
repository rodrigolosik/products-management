using Infrastructure.Repositories;
using Infrastructure.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System.Data;

namespace Infrastructure;

public static class RegisterServices
{
    public static void AddInfrasctrucutreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<AccessManager>()
            .AddSingleton<IDbConnection>(x => new MySqlConnection(configuration.GetConnectionString("Default")))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IAccessManager, AccessManager>();
    }
}
