using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Article : Entity
{
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public required Journal Journal { get; init; }

    private readonly List<Asset> assets = new();
    public IReadOnlyList<Asset> Assets => assets.AsReadOnly();
    public List<ArticleActor> Actors { get; private set; } = new();
}
