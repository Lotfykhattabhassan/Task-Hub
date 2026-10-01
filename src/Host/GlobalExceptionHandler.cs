using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Host;

// بيحول الـ exceptions المعروفة لـ status codes مظبوطة بدل 500
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails? problem = exception switch
        {
            ValidationException validation => new ValidationProblemDetails(
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed"
            },

            ForbiddenException => new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = exception.Message
            },

            NotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = exception.Message
            },

            ConflictException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = exception.Message
            },

            DomainException or ArgumentException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = exception.Message
            },

            _ => null
        };

        if (problem is null)
            return false; // 500 عادي

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            problem.GetType(),
            cancellationToken);

        return true;
    }
}
