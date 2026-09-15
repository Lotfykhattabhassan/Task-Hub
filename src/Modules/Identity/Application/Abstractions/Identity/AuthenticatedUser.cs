
namespace TaskHub.Modules.Identity.Application.Abstractions.Identity
{
    public sealed record AuthenticatedUser(
    Guid UserId,
    string Email);
}
