namespace Submission.Domain.Entities;

public partial class Journal
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abreviation { get; set; }
    private readonly List<Articles> _articles = new();
    public IReadOnlyCollection<Articles> Articles => _articles.AsReadOnly();
}
