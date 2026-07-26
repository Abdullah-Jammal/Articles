namespace Submission.Domain.ValueObjects;

public class AssetName : StringValueObject
{
    private AssetName(string value) => Value = value;

    public static AssetName FromAssetType(AssetType assetType)
    {
        string value = assetType.ToString();
        return new AssetName(value);
    }
}
