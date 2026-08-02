using System.ComponentModel.DataAnnotations;

namespace FileStorage.MongoGridFS;

public class MongoGridFsFileStorageOption
{
    [Required]
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
    public string BucketName { get; init; } = "files";
    public int ChunkSizeBytes { get; init; } = 1048576;
    public long FileSizeLimitInMB { get; init; } = 50;
    public long FileSizeLimitInBytes => FileSizeLimitInMB * 1024 * 1024;
}
