using Article.Security;
using Auth.Domain.Users;
using Blocks.AspNetCore;
using Blocks.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Application;

public class TokenFactory(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
{
    public RefreshToken GenerateRefreshToken()
    {
        var context = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No active HTTP request.");

        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpireOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
                CreatedByIp = context.GetClientIpAddress(),
            };
        }
    }

    public string GenerateJwtToken(User user, IEnumerable<string> roles, IEnumerable<Claim> additionalCalims)
    {
        var jwtSettings = jwtOptions.Value;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),

            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        }
        .Concat(roles.Select(role => new Claim(ClaimTypes.Role, role)))
        .Concat(additionalCalims);

        var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("YourSuperSecretKeyHere"));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ValidForMinutes),
            signingCredentials: signingCredentials
        );
        var encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return encodedJwtToken;
    }
}
