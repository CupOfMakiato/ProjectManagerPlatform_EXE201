using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Services;
using Server.Application.Utils;
namespace Server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddSingleton<TokenGenerators>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
