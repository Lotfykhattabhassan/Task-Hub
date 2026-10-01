using MediatR;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdQuery(Guid Id) : IRequest<GetTenantByIdResponse?>;
