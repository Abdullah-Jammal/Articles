using Articles.Abstractions.Enums;

namespace Submission.Domain.Entities;

public class Articles
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public required Journal Journal { get; init; }
}
