using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantResponse(
    Guid Id,
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    TenantStatus Status);
