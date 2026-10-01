using MediatR;
using TaskHub.Modules.Tasks.Application.Abstractions;
using TaskHub.Modules.Tasks.Application.DTOs;
using TaskHub.Modules.Tasks.Domain.Entities;

namespace TaskHub.Modules.Tasks.Application.Features.Tasks.Commands.CreateTask;

public sealed class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITasksUnitOfWork _unitOfWork;

    public CreateTaskHandler(ITaskRepository taskRepository, ITasksUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskResponse> Handle(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = TaskItem.Create(command.Title, command.Description);

        await _taskRepository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TaskResponse(task.Id, task.Title, task.Description, task.IsCompleted);
    }
}
