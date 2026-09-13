using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class ArticleAuthorEntityConfiguration : IEntityTypeConfiguration<ArticleAuthor>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ArticleAuthor> builder)
    {
        builder.Property(a => a.ContributionAreas)
            .HasJsonCollectionConversion().IsRequired();
    }
}
