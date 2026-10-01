using TaskHub.Modules.Tasks.Domain.Entities;

namespace TaskHub.Modules.Tasks.Application.Abstractions;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);
}
