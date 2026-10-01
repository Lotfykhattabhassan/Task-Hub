using MediatR;
using TaskHub.Modules.Tasks.Application.Abstractions;
using TaskHub.Modules.Tasks.Application.DTOs;

namespace TaskHub.Modules.Tasks.Application.Features.Tasks.Queries.GetTasks;

public sealed class GetTasksHandler : IRequestHandler<GetTasksQuery, IReadOnlyList<TaskResponse>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskResponse>> Handle(
        GetTasksQuery query,
        CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllAsync(cancellationToken);

        return tasks
            .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsCompleted))
            .ToList();
    }
}
