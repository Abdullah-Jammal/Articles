using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Journal : Entity
{
    public required string Name { get; set; }
    public required string Abreviation { get; set; }
    private readonly List<Article> _articles = new();
    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();
}
