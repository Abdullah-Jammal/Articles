namespace Submission.Domain.ValueObjects;

public class FileExtension : StringValueObject
{
    private FileExtension(string value) => Value = value;
    public static FileExtension FromFileName(string fileName, AssetTypeDefinition assetType)
    {
        var extension = Path.GetExtension(fileName).Remove(0, 1);
        Guard.ThrowIfNullOrWhiteSpace(extension);

        ArgumentOutOfRangeException.ThrowIfNotEqual(
            assetType.AllowedFileExtension.IsValidExtension(extension), true);

        return new FileExtension(extension);
    }
}
