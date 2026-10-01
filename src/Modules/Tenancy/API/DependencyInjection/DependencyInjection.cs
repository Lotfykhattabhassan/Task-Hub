using Microsoft.Extensions.DependencyInjection;

namespace TaskHub.Modules.Tenancy.Api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddTenancyApi(this IServiceCollection services)
    {
        return services;
    }
}
