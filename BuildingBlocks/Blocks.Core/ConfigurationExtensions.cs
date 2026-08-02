using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blocks.Core;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAndValidateOptions<TOptions>(this IServiceCollection services,
        IConfiguration configuration) where TOptions : class
    {
        var section = configuration.GetSection(typeof(TOptions).Name);
        if (!section.Exists())
            throw new InvalidOperationException($"Configuration section '{section.Key}' does not exist.");
        services
            .AddOptions<TOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
}
