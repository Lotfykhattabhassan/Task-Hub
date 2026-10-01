using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenants;

public sealed record GetTenantsResponse(
    Guid Id,
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    TenantStatus Status);
