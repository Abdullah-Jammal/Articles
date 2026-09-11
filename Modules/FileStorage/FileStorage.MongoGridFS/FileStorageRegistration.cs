using Blocks.Core;
using FileStorage.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.MongoGridFS;

public static class FileStorageRegistration
{
    public static IServiceCollection AddMongoFsFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(MongoGridFsFileStorageOption.SectionName);

        if (!section.Exists())
            throw new InvalidOperationException($"configuration section '{section.Key}' does not exist");

        services
            .AddOptions<MongoGridFsFileStorageOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var options = section.Get<MongoGridFsFileStorageOption>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{section.Key}' was not found.");

        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(configuration.GetConnectionStringOrThrow(options.ConnectionStringName));
        });

        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        services.AddSingleton(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = options.BucketName,
                ChunkSizeBytes = options.ChunkSizeBytes,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Primary
            });
        });

        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}
