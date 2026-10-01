using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Modules.Tasks.Application.DTOs;
using TaskHub.Modules.Tasks.Application.Features.Tasks.Commands.CreateTask;
using TaskHub.Modules.Tasks.Application.Features.Tasks.Queries.GetTasks;

namespace TaskHub.Modules.Tasks.Api.Controllers;

// محتاج Authorization header و X-Tenant-Id (الـ middleware بيتحقق منهم)
[ApiController]
[Authorize]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly ISender _sender;

    public TasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetAll(
        CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetTasksQuery(), cancellationToken));

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateTaskCommand(request.Title, request.Description),
            cancellationToken);

        return Ok(response);
    }
}

public sealed record CreateTaskRequest(string Title, string? Description);
