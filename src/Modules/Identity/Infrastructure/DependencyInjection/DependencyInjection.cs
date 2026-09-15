using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Application.Abstractions.Persistence;
using TaskHub.Modules.Identity.Infrastructure.Authentication;
using TaskHub.Modules.Identity.Infrastructure.Configuration;
using TaskHub.Modules.Identity.Infrastructure.Identity;
using TaskHub.Modules.Identity.Infrastructure.Persistence;

namespace TaskHub.Modules.Identity.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("TaskHub");

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(connectionString));

        services
            .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));
            
        services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddHttpContextAccessor();

        return services;
    }
}