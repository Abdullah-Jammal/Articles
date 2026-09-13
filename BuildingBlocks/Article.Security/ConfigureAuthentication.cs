using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Article.Security;

public static class ConfigureAuthentication
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
            ?? throw new InvalidOperationException("Configuration section 'JwtOptions' does not exist.");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey))
                };
            });
        return services;
    }
}
