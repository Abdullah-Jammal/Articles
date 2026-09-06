using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Article.Security;

public static class ConfigureAuthentication
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddBearerToken(jwtOptions =>
            {
                // Configure JWT options here
            });
        return services;
    }
}
