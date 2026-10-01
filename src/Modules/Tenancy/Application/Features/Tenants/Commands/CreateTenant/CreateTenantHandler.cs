using MediatR;
using TaskHub.BuildingBlocks.Application.Abstractions;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.Enums;
using TaskHub.Modules.Tenancy.Domain.ValueObjects;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantHandler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly IConnectionStringProtector _connectionStringProtector;
    private readonly IEnumerable<ITenantDatabaseProvisioner> _provisioners;

    public CreateTenantHandler(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserAccessor currentUser,
        IConnectionStringProtector connectionStringProtector,
        IEnumerable<ITenantDatabaseProvisioner> provisioners)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _connectionStringProtector = connectionStringProtector;
        _provisioners = provisioners;
    }

    public async Task<CreateTenantResponse> Handle(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenException("User is not authenticated.");

        var slug = TenantSlug.Create(command.Slug);

        if (await _tenantRepository.ExistsBySlugAsync(slug.Value, cancellationToken))
            throw new ConflictException($"Tenant slug '{slug.Value}' already exists.");

        var connectionString = command.StorageMode == TenantStorageMode.Dedicated
            ? command.ConnectionString?.Trim()
            : null;

        // Dedicated: جهّز الـ DB الجديدة (migrations) قبل ما نسجل الـ tenant
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            foreach (var provisioner in _provisioners)
                await provisioner.ProvisionAsync(connectionString, cancellationToken);
        }

        var tenant = Tenant.Create(
            TenantName.Create(command.Name),
            slug,
            command.StorageMode,
            connectionString is null
                ? null
                : _connectionStringProtector.Protect(connectionString));

        // اللي عمل الـ tenant بيبقى Owner وعضويته Active على طول
        var owner = TenantMembership.Create(userId, tenant.Id, TenantMemberRole.Owner);
        owner.Accept();
        tenant.AddMember(owner);

        await _tenantRepository.AddAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateTenantResponse(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.StorageMode,
            tenant.Status);
    }
}
