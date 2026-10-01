using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.Modules.Tasks.Application.Abstractions;
using TaskHub.Modules.Tasks.Infrastructure.Persistence;
using TaskHub.Modules.Tasks.Infrastructure.Repositories;

namespace TaskHub.Modules.Tasks.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddTasksInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<TenantDataDbContext>((sp, options) =>
        {
            if (EF.IsDesignTime)
            {
                options.UseTenantDataSqlServer(
                    "Server=.;Database=TaskHubDb;Trusted_Connection=True;TrustServerCertificate=True");
                return;
            }

            var resolver = sp.GetRequiredService<ITenantConnectionStringResolver>();
            options.UseTenantDataSqlServer(resolver.Resolve());
        });

        services.AddScoped<ITasksUnitOfWork, TasksUnitOfWork>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITenantDatabaseProvisioner, TasksDatabaseProvisioner>();

        return services;
    }
}
