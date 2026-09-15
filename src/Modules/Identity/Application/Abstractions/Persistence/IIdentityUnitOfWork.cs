
namespace TaskHub.Modules.Identity.Application.Abstractions.Persistence
{
    public interface IIdentityUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
