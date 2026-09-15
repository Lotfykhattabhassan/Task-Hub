using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;

namespace TaskHub.Modules.Identity.Infrastructure.Identity;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(JwtRegisteredClaimNames.Sub)?
                .Value;

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? Email
    {
        get
        {
            return _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(JwtRegisteredClaimNames.Email)?
                .Value;
        }
    }
}