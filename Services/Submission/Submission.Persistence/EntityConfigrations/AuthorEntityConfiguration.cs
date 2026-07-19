using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class AuthorEntityConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Degreed)
            .HasMaxLength(100)
            .HasComment("The author's highest degree or qualification.");
        builder.Property(a => a.Discipline)
            .HasMaxLength(100)
            .HasComment("The discipline or field of study of the author.");
    }
}
