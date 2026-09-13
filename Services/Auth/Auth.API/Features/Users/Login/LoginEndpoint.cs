using Auth.Application;
using Blocks.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace Auth.API.Features.Users.Login;

[AllowAnonymous]
[HttpPost("login")]
public class LoginEndpoint(UserManager<User> userManager,
    SignInManager<User> signInManager, TokenFactory tokenFactory) : Endpoint<LoginCommand, LoginResponse>
{
    public override async Task HandleAsync(LoginCommand command, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            ThrowError("Invalid email or password.", (int)HttpStatusCode.BadRequest);

        var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new BadRequestException($"Invalid email or password.");

        var userRoles = await userManager.GetRolesAsync(user);

        var jwtToken = tokenFactory.GenerateJwtToken(user, userRoles, new List<Claim>());
        var refreshToken = tokenFactory.GenerateRefreshToken();

        user.AddRefreshToken(refreshToken);

        await Send.OkAsync(new LoginResponse(command.Email, jwtToken, refreshToken.Token), ct);
    }
}
