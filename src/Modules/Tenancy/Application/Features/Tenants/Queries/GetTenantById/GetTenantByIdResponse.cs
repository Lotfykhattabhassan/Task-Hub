using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdResponse(
    Guid Id,
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    TenantStatus Status);
