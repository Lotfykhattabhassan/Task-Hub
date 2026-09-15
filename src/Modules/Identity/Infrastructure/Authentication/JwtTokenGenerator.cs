using Microsoft.Extensions.Options;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Infrastructure.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
namespace TaskHub.Modules.Identity.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;

            if (string.IsNullOrWhiteSpace(_jwtOptions.SecretKey))
                throw new InvalidOperationException(
                    "JWT SecretKey is missing from configuration.");
        }
        public JwtTokenResult GenerateToken(Guid userId, string email)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
            
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtTokenResult
            (accessToken, expiresAt);
        }
    }
}
