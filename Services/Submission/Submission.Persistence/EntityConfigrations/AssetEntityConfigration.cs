using Blocks.EntityFramework;
using Blocks.EntityFramework.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class AssetEntityConfigration : EntityConfigration<Asset>
{
    public override void Configure(EntityTypeBuilder<Asset> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Type).IsRequired().HasEnumConversion();
        builder.ComplexProperty(
            o => o.Name, builder =>
            {
                builder.Property(n => n.Value)
                .HasColumnName(builder.Metadata.PropertyInfo!.Name)
                .HasMaxLength(maxLength: 100).IsRequired();
            });

        builder.ComplexProperty(e => e.File, fileBuilder =>
        {
            new FileEntityConfigration().Configure(fileBuilder);
        });
    }
}
