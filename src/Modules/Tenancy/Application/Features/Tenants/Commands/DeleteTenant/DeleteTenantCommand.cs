using MediatR;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.DeleteTenant;

public sealed record DeleteTenantCommand(Guid Id) : IRequest<DeleteTenantResponse?>;
