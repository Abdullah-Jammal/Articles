using Microsoft.AspNetCore.Http;

namespace Blocks.AspNetCore;

public static class Extensions
{
    public static string? BaseUrl(this HttpRequest request)
    {
        if (request == null) return null;
        var userBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? (request.IsHttps ? 443 : 80)
        };
        if (userBuilder.Uri.IsDefaultPort)
        {
            userBuilder.Port = -1;
        }
        return userBuilder.Uri.AbsoluteUri;
    }

    public static string GetClientIpAddress(this HttpContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        // Check for X-Forwarded-For header first (in case of reverse proxy)
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ip = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
        }
        // Fallback to the remote IP address
        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
