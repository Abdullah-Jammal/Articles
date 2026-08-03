using FileStorage.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.MongoGridFS;

public class FileService : IFileService
{
    private readonly GridFSBucket _bucket;
    private readonly MongoGridFsFileStorageOption _options;
    private const string FilePathMetadataKey = "FilePath";
    private const string ContentTypeMetadataKey = "ContentType";

    public FileService(GridFSBucket bucket, IOptions<MongoGridFsFileStorageOption> options)
        => (_bucket, _options) = (bucket, options.Value);

    public async Task<UploadResponse> UploadFileAsync(string filePath,
       IFormFile file, bool overwrite = false, Dictionary<string, string>? tags = null)
    {
        if(file.Length > _options.FileSizeLimitInBytes)
            throw new InvalidOperationException($"File size exceeds the limit of {_options.FileSizeLimitInBytes} bytes.");

        var metadata = new BsonDocument(tags ?? new Dictionary<string, string>())
        {
            { FilePathMetadataKey, filePath },
            { ContentTypeMetadataKey, file.ContentType }
        };
        
        var uploadOptions = new GridFSUploadOptions
        {
            Metadata = metadata,
            ChunkSizeBytes = _options.ChunkSizeBytes
        };

        ObjectId fileId;
        using (var stream = file.OpenReadStream())
        {
            fileId = await _bucket.UploadFromStreamAsync(file.FileName, stream, uploadOptions);
        }

        return new UploadResponse
        (
            FilePath: filePath,
            FileName: file.FileName,
            FileSize: file.Length,
            FileId: fileId.ToString()
        );
    }

    public async Task<(Stream FileStream, string ContentType)> DownloadFileAsync(string fileId)
    {
        if (!ObjectId.TryParse(fileId, out var objectId))
            throw new FileNotFoundException("Invalid file ID format.", nameof(fileId));

        var fileInfo = await _bucket.Find(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        if (fileInfo == null)
            throw new FileNotFoundException("File not found.", nameof(fileId));

        var stream = await _bucket.OpenDownloadStreamAsync(objectId);
        var contentType = fileInfo.Metadata?.GetValue(ContentTypeMetadataKey)?.AsString ?? "application/octet-stream";
        return (stream, contentType);
    }

    public async Task<bool> TryDeleteFileAsync(string fileId)
    {
        if (!ObjectId.TryParse(fileId, out var objectId))
            return false;

        try
        {
            await _bucket.DeleteAsync(objectId);
            return true;
        }
        catch (GridFSFileNotFoundException)
        {
            return false;
        }
    }
}
