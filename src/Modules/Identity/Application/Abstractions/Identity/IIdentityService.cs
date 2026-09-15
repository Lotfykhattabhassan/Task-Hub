using TaskHub.Modules.Identity.Domain.Entities;
using TaskHub.Modules.Identity.Domain.ValueObjects;

namespace TaskHub.Modules.Identity.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<IdentityOperationResult> CreateUserAsync(
        User user,
        string phoneNumber,
        string password);
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AuthenticatedUser?> ValidateCredentialsAsync(
    Email email,
    string password);
}
