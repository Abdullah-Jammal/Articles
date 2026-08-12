using Blocks.Domain;

namespace Submission.Domain.Entities;

public partial class Article
{
    // HashSet : is used because it does not allow duplicate values.
    public void AssignAuthor(Author author, HashSet<ContributionArea> contributionAreas, bool isCorrespondingAuthor)
    {
        ArgumentNullException.ThrowIfNull(author);
        ArgumentNullException.ThrowIfNull(contributionAreas);

        if (contributionAreas.Count == 0)
            throw new DomainException("At least one contribution area is required.");

        var role = isCorrespondingAuthor ? UserRoleType.CORAUT : UserRoleType.AUT;

        if (Actors.Exists(actor => actor.PersonId == author.Id))
            throw new DomainException($"Author {author.EmailAddress} is already assigned to the article.");

        Actors.Add(new ArticleAuthor()
        {
            ContributionAreas = contributionAreas,
            Person = author,
            Role = role,
        });
    }

    public Asset CreateAsset(AssetTypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var assetCount = assets.Count(asset => asset.Type == type.Name);

        if (assetCount >= type.MaxAssetCount)
            throw new DomainException(
                $"The maximum number of files allowed for {type.Name} was already reached.");

        var asset = Asset.Create(this, type);
        assets.Add(asset);
        return asset;
    }
}
