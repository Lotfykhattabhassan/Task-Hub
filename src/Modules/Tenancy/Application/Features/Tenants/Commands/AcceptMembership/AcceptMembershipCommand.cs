using MediatR;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AcceptMembership;

public sealed record AcceptMembershipCommand(Guid TenantId) : IRequest;
