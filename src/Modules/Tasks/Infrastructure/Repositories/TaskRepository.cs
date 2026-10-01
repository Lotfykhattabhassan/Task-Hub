using Microsoft.EntityFrameworkCore;
using TaskHub.Modules.Tasks.Application.Abstractions;
using TaskHub.Modules.Tasks.Domain.Entities;
using TaskHub.Modules.Tasks.Infrastructure.Persistence;

namespace TaskHub.Modules.Tasks.Infrastructure.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly TenantDataDbContext _context;

    public TaskRepository(TenantDataDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default)
    {
        await _context.TaskItems.AddAsync(task, cancellationToken);
    }
}
