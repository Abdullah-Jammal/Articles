using FileStorage.Contracts;

namespace Submission.Application.Features.UploadFile.UploadManuscriptFile;

public class UploadManuscriptFileCommandHandler(
    ArticleRepository articleRepository,
    Repository<AssetTypeDefinition> assetTypeDefinition,
    IFileService fileService)
    : IRequestHandler<UploadManuscriptFileCommand, IdResponse>
{
    public async Task<IdResponse> Handle(
        UploadManuscriptFileCommand command,
        CancellationToken cancellationToken)
    {
        var article = await articleRepository.GetByIdOrThrowAsync(command.ArticleId, cancellationToken);
        var assetType = await assetTypeDefinition.FindByIdAsync((int)command.AssetType, cancellationToken)
            ?? throw new InvalidOperationException($"Asset type '{command.AssetType}' was not found.");

        Asset? asset = null;

        if (!assetType.AllowsMultipleAssets)
            asset = article.Assets.SingleOrDefault(e => e.Type == assetType.Name);

        if (asset == null)
            asset = article.CreateAsset(assetType);

        var filePath = asset.GenerateStorageFilePath(command.File.FileName);
        var uploadResponse = await fileService.UploadFileAsync(filePath, command.File,
            overwrite: true,
            tags: new Dictionary<string, string>
        {
            { "entity", nameof(Asset) },
            { "entityId", asset.Id.ToString() },
        });

        try
        {
            asset.CreateFile(uploadResponse, assetType);

            await articleRepository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await fileService.TryDeleteFileAsync(uploadResponse.FileId);
            throw;
        }

        return new IdResponse(asset.Id);
    }
}
