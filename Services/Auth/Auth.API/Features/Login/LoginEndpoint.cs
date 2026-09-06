using Article.Security;
using Blocks.AspNetCore;
using Blocks.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.API.Features.Login;

[HttpPost("login")]
public class LoginEndpoint(UserManager<User> userManager,
    SignInManager<User> signInManager,
    IOptions<JwtOptions> jwtOptions) : Endpoint<LoginCommand, LoginResponse>
{
    public override async Task HandleAsync(LoginCommand command, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if(user is null)
            ThrowError("Invalid email or password.", (int)HttpStatusCode.BadRequest);

        var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException($"Invalid email or password.");

        var userRoles = await userManager.GetRolesAsync(user);

        var jwtToken = GenerateJwtToken(user, userRoles, new List<Claim>());
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken.Add(refreshToken);

        await Send.OkAsync(new LoginResponse(command.Email, jwtToken, refreshToken.Token), ct);
    }

    public RefreshToken GenerateRefreshToken()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token  = Convert.ToBase64String(randomBytes),
                ExpireOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
                CreatedByIp = HttpContext.GetClientIpAddress(),
            };
        };
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
