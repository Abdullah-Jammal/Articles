using System.Security.Claims;
using Blocks.Domain;
using Microsoft.AspNetCore.Http;

namespace Article.Security;

public sealed class HttpCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int UserId
    {
        get
        {
            var principal = httpContextAccessor.HttpContext?.User;
            var identifier = principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal?.FindFirstValue("sub");

            return int.TryParse(identifier, out var userId) && userId > 0
                ? userId
                : throw new UnauthorizedAccessException("The authenticated user has no valid numeric identifier.");
        }
    }
}
