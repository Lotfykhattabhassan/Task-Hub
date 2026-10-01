using TaskHub.Modules.Tasks.Application.Abstractions;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

public sealed class TasksUnitOfWork : ITasksUnitOfWork
{
    private readonly TenantDataDbContext _context;

    public TasksUnitOfWork(TenantDataDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
