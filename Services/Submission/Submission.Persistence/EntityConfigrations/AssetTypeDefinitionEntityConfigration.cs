using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class AssetTypeDefinitionEntityConfigration : IEntityTypeConfiguration<AssetTypeDefinition>
{
    public void Configure(EntityTypeBuilder<AssetTypeDefinition> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(e => e.Name).HasEnumConversion().HasMaxLength(256).IsRequired().HasColumnOrder(1);
        builder.Property(e => e.MaxFileSizeInMB).HasMaxLength(8);
        builder.Property(e => e.DefaultFileExtension).HasMaxLength(8).HasDefaultValue("pdf").IsRequired();

        builder.ComplexProperty(e => e.AllowedFileExtension, builder =>
        {
            var convertor = BuilderExtensions.BuildJsonListConvertor<string>();
            builder.Property(e => e.Extensions).HasConversion(convertor)
            .HasColumnName("name")
            .IsRequired();
        });
    }
}
