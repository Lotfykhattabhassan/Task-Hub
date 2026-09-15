namespace TaskHub.Modules.Identity.Application.Abstractions.Identity
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
        string? Email { get; }
    }
}
