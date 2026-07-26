namespace Submission.Domain.Entities;

public class AssetTypeDefinition
{
    public AssetType Id { get; init; }
    public byte MaxFileSizeInMB { get; set; }
    public string DefaultFileExtension { get; set; } = default!;
    public string AllowedFileExtension { get; set; }
}
