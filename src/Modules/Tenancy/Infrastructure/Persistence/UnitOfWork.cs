using TaskHub.Modules.Tenancy.Application.Abstractions;

namespace TaskHub.Modules.Tenancy.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TenantRegistryDbContext _context;

    public UnitOfWork(TenantRegistryDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
