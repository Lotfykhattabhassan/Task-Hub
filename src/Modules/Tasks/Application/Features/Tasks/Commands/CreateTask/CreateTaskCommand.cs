using MediatR;
using TaskHub.Modules.Tasks.Application.DTOs;

namespace TaskHub.Modules.Tasks.Application.Features.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand(
    string Title,
    string? Description) : IRequest<TaskResponse>;
