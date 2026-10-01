using Microsoft.Extensions.DependencyInjection;
using TaskHub.BuildingBlocks.Application.Abstractions;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.BuildingBlocks.Infrastructure.Identity;

namespace TaskHub.BuildingBlocks.Infrastructure.MultiTenancy;

public static class DependencyInjection
{
    public static IServiceCollection AddTenantInfrastructure(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        services.AddScoped<TenantContext>();

        services.AddScoped<ITenantContext>(
            sp => sp.GetRequiredService<TenantContext>());

        services.AddScoped<ITenantContextSetter>(
            sp => sp.GetRequiredService<TenantContext>());

        services.AddScoped<ITenantConnectionStringResolver,
            TenantConnectionStringResolver>();

        return services;
    }
}
