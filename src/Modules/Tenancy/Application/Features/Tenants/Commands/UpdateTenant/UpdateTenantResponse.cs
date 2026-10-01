using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.UpdateTenant;

public sealed record UpdateTenantResponse(
    Guid Id,
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    TenantStatus Status);
