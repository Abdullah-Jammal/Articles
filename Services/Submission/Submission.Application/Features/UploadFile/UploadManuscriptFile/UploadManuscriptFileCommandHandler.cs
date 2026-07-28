namespace Submission.Application.Features.UploadFile.UploadManuscriptFile;

public class UploadManuscriptFileCommandHandler(ArticleRepository articleRepository,
    Repository<AssetTypeDefinition> assetTypeDefinition)
    : IRequestHandler<UploadManuscriptFileCommand, IdResponse>
{
    public async Task<IdResponse> Handle(UploadManuscriptFileCommand command, CancellationToken cancellationToken)
    {
        var article = await articleRepository.GetByIdOrThrowAsync(command.ArticleId, cancellationToken);
        var assetType = await assetTypeDefinition.FindByIdAsync((int)command.AssetType, cancellationToken)
            ?? throw new InvalidOperationException($"Asset type '{command.AssetType}' was not found.");

        Asset? asset = null;

        if (!assetType.AllowsMultipleAssets)
            asset = article.Assets.SingleOrDefault(e => e.Type == assetType.Name);

        if (asset == null)
            asset = article.CreateAsset(assetType);

        await articleRepository.SaveChangesAsync(cancellationToken);

        return new IdResponse(asset.Id);
    }
}
