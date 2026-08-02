using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.MongoGridFS;

public static class FileStorageRegistration
{
    public static IServiceCollection AddMongoFsFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(MongoGridFsFileStorageOption));

        if (!section.Exists())
            throw new InvalidOperationException($"configuration section '{section.Key}' does not exist");

        services
            .AddOptions<MongoGridFsFileStorageOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
