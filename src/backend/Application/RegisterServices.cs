using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class RegisterServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
    }
}
