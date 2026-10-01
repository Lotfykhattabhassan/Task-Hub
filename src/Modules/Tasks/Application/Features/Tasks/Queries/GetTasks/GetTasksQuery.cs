using MediatR;
using TaskHub.Modules.Tasks.Application.DTOs;

namespace TaskHub.Modules.Tasks.Application.Features.Tasks.Queries.GetTasks;

public sealed record GetTasksQuery : IRequest<IReadOnlyList<TaskResponse>>;
