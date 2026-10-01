using TaskHub.Modules.Tenancy.Domain.Entities;

namespace TaskHub.Modules.Tenancy.Application.Abstractions;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    // الـ tenants اللي المستخدم عضو Active فيها
    Task<IReadOnlyCollection<Tenant>> GetByMemberUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Tenant?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Tenant tenant,
        CancellationToken cancellationToken = default);

}