using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.BuildingBlocks.Application.Behaviors;

namespace TaskHub.Modules.Identity.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            cfg.AddOpenBehavior(
                typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
