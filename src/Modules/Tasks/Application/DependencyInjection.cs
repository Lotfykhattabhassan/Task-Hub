using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.BuildingBlocks.Application.Behaviors;

namespace TaskHub.Modules.Tasks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTasksApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
