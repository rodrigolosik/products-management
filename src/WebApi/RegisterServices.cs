using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApi;

public static class RegisterServices
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurando o uso da classe de contexto para
        // acesso às tabelas do ASP.NET Identity Core
        services.AddDbContext<ApiSecurityDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDatabase"));

        services.AddDbContext<ApiSecurityDbContext>(optionsAction =>
        {
            optionsAction.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        });

        var tokenConfigurations = new TokenConfigurations();

        new ConfigureFromConfigurationOptions<TokenConfigurations>(
            configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);

        // Aciona a extensão que irá configurar o uso de
        // autenticação e autorização via tokens
        services.AddJwtSecurity(tokenConfigurations);

        // Acionar caso seja necessário criar usuários para testes
        services.AddScoped<IdentityInitializer>();
    }
}
