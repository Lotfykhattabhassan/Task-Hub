using MediatR;
using TaskHub.BuildingBlocks.Application.Abstractions;
using TaskHub.Modules.Tenancy.Application.Abstractions;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenants;

// بيرجّع الـ tenants اللي المستخدم الحالي عضو Active فيها بس
public sealed class GetTenantsHandler : IRequestHandler<GetTenantsQuery, IReadOnlyList<GetTenantsResponse>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentUserAccessor _currentUser;

    public GetTenantsHandler(
        ITenantRepository tenantRepository,
        ICurrentUserAccessor currentUser)
    {
        _tenantRepository = tenantRepository;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<GetTenantsResponse>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return new List<GetTenantsResponse>();

        var tenants = await _tenantRepository.GetByMemberUserIdAsync(
            userId.Value,
            cancellationToken);

        return tenants
            .Select(tenant => new GetTenantsResponse(
                tenant.Id,
                tenant.Name.Value,
                tenant.Slug.Value,
                tenant.StorageMode,
                tenant.Status))
            .ToList();
    }
}
