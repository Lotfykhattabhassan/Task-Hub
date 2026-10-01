using MediatR;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.UpdateTenant;

public sealed record UpdateTenantCommand(
    Guid Id,
    string Name,
    string Slug,
    TaskHub.Modules.Tenancy.Domain.Enums.TenantStatus Status)
    : IRequest<UpdateTenantResponse?>;
