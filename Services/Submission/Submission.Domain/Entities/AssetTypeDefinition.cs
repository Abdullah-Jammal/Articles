using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public class AssetTypeDefinition : EnumEntity<AssetType>
{
    public required byte MaxFileSizeInMB { get; init; }
    public int MaxFileSizeInByte => (MaxFileSizeInMB * 1024 * 1024);
    public required string DefaultFileExtension { get; init; } = default!;
    public required FileExtensions AllowedFileExtension { get; init; }
    public int MaxAssetCount { get; init; }
    public bool AllowsMultipleAssets => MaxAssetCount > 1;
}
