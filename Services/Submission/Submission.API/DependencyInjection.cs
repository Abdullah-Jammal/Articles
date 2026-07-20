namespace Submission.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<Blocks.Domain.ICurrentUser, Article.Security.HttpCurrentUser>();

        services
            .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => configuration.GetSection("Authentication").Bind(options));

        services
            .AddMemoryCache()
            .AddAuthorization()
            .AddProblemDetails()
            .AddExceptionHandler<ApiExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}
