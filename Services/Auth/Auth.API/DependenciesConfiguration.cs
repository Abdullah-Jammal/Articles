using IdentityRole = Auth.Domain.Role.Role;
using FastEndpoints.Swagger;
using System.Security.Claims;
using Article.Security;
using EmailService.Smtp;
using Auth.Persistence;
using Auth.Application;
using Microsoft.AspNetCore.Identity;

namespace Auth.API;

public static class DependenciesConfiguration
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAndValidateOptions<JwtOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddApiService(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<TokenFactory>();

        services.AddFastEndpoints()
            .SwaggerDocument()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddAuthentication(config)
            .AddJwtIdentity(config)
            .AddAuthorization();

        services.AddSmtpEmailService(config);
        return services;
    }

    public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<User>(options =>
        {
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDBContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();
        return services;
    }
}
