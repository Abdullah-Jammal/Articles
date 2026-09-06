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
}
