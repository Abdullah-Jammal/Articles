using Articles.Abstractions.Enums;
using Microsoft.AspNetCore.Builder;

namespace Article.Security;

public static class Extensions
{
    public static TBuilder RequireRoleAuthorization<TBuilder>(this TBuilder builder, params string[] roles)
        where TBuilder : IEndpointConventionBuilder
        => builder.RequireAuthorization(policy => policy.RequireRole(roles));

    public static TBuilder RequireRoleAuthorization<TBuilder>(this TBuilder builder, params UserRoleType[] roles)
        where TBuilder : IEndpointConventionBuilder
        => builder.RequireAuthorization(policy => policy.RequireRole(roles.Select(role => role.ToString())));
}
