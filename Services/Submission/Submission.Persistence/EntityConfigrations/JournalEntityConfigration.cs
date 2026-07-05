using Blocks.EntityFramework.EntityConfigrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class JournalEntityConfigration : EntityConfigration<Journal>
{
    public override void Configure(EntityTypeBuilder<Journal> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(64);
        builder.Property(e => e.Abreviation).IsRequired().HasMaxLength(8);
    }
}
