namespace TaskHub.Modules.Identity.Application.Abstractions.Identity;

public sealed record JwtTokenResult(
    string AccessToken,
    DateTime ExpiresAt);
