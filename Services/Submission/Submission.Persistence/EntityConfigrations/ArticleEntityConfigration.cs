using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class ArticleEntityConfigration : EntityConfigration<Article>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Scope).IsRequired().HasMaxLength(2048);

        builder.Property(e => e.Stage).HasEnumConversion();
        builder.Property(e => e.Type).HasEnumConversion();

        builder.HasOne(e => e.Journal).WithMany(e => e.Articles)
            .HasForeignKey(e => e.JournalId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Assets).WithOne(e => e.Article)
            .HasForeignKey(e => e.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
