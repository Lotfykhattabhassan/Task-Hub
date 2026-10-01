using FluentValidation;
using TaskHub.BuildingBlocks.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Application.Security;

namespace TaskHub.Modules.Tenancy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTenancyApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ITenantAccessGuard, TenantAccessGuard>();

        return services;
    }
}
