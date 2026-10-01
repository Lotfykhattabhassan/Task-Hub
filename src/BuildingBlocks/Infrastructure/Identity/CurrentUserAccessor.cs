using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskHub.BuildingBlocks.Application.Abstractions;

namespace TaskHub.BuildingBlocks.Infrastructure.Identity;

public sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var value = user?.FindFirst("sub")?.Value
                        ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}
