using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Infrastructure.MultiTenancy;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantRepository tenantRepository,
        ITenantMembershipRepository tenantMembershipRepository,
        ITenantContextSetter tenantContextSetter,
        IConnectionStringProtector connectionStringProtector)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null ||
            endpoint?.Metadata.GetMetadata<SkipTenantResolutionAttribute>() is not null)
        {
            await _next(context);
            return;
        }

        var cancellationToken = context.RequestAborted;
        var tenantIdValue = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();

        if (!Guid.TryParse(tenantIdValue, out var tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var userIdClaim = context.User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var tenant = await tenantRepository.GetByIdAsync(
            tenantId,
            cancellationToken);

        if (tenant is null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        if (tenant.Status != TenantStatus.Active)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var membership = await tenantMembershipRepository
            .GetByTenantAndUserAsync(
                tenantId,
                userId,
                cancellationToken);

        if (membership is null || membership.Status != MembershipStatus.Active)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var storageType = tenant.StorageMode switch
        {
            TenantStorageMode.Shared => TenantStorageType.Shared,
            TenantStorageMode.Dedicated => TenantStorageType.Dedicated,
            _ => throw new InvalidOperationException(
                "Unsupported tenant storage mode.")
        };

        var connectionString = tenant.ConnectionString is null
            ? null
            : connectionStringProtector.Unprotect(tenant.ConnectionString);

        tenantContextSetter.SetTenantContext(
            tenant.Id,
            storageType,
            connectionString);

        await _next(context);
    }
}
