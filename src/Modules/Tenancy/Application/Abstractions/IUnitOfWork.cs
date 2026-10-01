namespace TaskHub.Modules.Tenancy.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}