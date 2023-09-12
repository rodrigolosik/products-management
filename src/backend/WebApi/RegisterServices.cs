using Application.Services;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi;

public static class RegisterServices
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddOptions<TokenConfiguration>()
            .BindConfiguration(nameof(TokenConfiguration))
            .ValidateOnStart();

        var tokenConfiguration = new TokenConfiguration();

        new ConfigureFromConfigurationOptions<TokenConfiguration>(
            configuration.GetSection("TokenConfiguration"))
            .Configure(tokenConfiguration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenConfiguration.Issuer,
                ValidAudience = tokenConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.SecretJwtKey))
            };
        });
    }
}