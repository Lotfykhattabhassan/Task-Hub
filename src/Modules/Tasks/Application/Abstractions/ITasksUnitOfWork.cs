namespace TaskHub.Modules.Tasks.Application.Abstractions;

public interface ITasksUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
