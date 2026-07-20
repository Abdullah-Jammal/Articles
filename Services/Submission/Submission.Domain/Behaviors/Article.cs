using Articles.Abstractions.Enums;
using Blocks.Domain;

namespace Submission.Domain.Entities;

public partial class Article
{
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
            Article = this,
            ArticleId = Id,
            ContributionAreas = [.. contributionAreas],
            Person = author,
            PersonId = author.Id,
            Role = role,
        });
    }
}
