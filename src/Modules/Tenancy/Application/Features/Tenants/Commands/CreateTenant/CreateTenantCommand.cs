using MediatR;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    string? ConnectionString) : IRequest<CreateTenantResponse>;
