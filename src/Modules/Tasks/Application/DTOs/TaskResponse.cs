namespace TaskHub.Modules.Tasks.Application.DTOs;

public sealed record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted);
