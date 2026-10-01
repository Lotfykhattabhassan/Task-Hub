namespace TaskHub.BuildingBlocks.Application.Exceptions;

// 403
public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}

// 409
public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}

// 404
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
