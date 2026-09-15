namespace TaskHub.Modules.Identity.Application.Abstractions.Identity;

public interface IJwtTokenGenerator
{
    JwtTokenResult GenerateToken(Guid userId, string email);
}
