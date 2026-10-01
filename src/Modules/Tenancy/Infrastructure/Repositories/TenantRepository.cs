using Microsoft.EntityFrameworkCore;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.Enums;
using TaskHub.Modules.Tenancy.Infrastructure.Persistence;

namespace TaskHub.Modules.Tenancy.Infrastructure.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly TenantRegistryDbContext _context;

    public TenantRepository(TenantRegistryDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> GetByIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(
                x => x.Id == tenantId,
                cancellationToken);
    }
    public async Task<IReadOnlyCollection<Tenant>> GetByMemberUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .Where(t => t.Members.Any(m =>
                m.UserId == userId &&
                m.Status == MembershipStatus.Active))
            .ToListAsync(cancellationToken);
    }
    public async Task<Tenant?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(
                x => x.Slug.Value == slug,
                cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .IgnoreQueryFilters()
            .AnyAsync(
                x => x.Slug.Value == slug,
                cancellationToken);
    }

    public async Task AddAsync(
        Tenant tenant,
        CancellationToken cancellationToken = default)
    {
        await _context.Tenants.AddAsync(
            tenant,
            cancellationToken);
    }

}