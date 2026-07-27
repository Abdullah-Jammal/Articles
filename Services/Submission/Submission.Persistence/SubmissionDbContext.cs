using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence;

public class SubmissionDbContext(DbContextOptions<SubmissionDbContext> options) : DbContext(options)
{
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<Journal> Journals { get; set; }
    public virtual DbSet<Person> Persons { get; set; }
    public virtual DbSet<ArticleActor> ArticleActors { get; set; }
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Asset> Assets { get; set; }
    public virtual DbSet<ArticleAuthor> ArticleAuthors { get; set; }
    public virtual DbSet<AssetTypeDefinition> AssetTypeDefinitions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
