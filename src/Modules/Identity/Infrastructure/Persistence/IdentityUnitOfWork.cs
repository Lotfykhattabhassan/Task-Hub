using TaskHub.Modules.Identity.Application.Abstractions.Persistence;

namespace TaskHub.Modules.Identity.Infrastructure.Persistence
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private readonly IdentityDbContext _dbContext;
        public IdentityUnitOfWork(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
