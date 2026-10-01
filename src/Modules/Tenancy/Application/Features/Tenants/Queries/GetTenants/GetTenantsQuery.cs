using MediatR;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery : IRequest<IReadOnlyList<GetTenantsResponse>>;
