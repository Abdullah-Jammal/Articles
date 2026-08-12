using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Submission.Persistence.EntityConfigrations;

internal class FileEntityConfigration
{
    public void Configure(ComplexPropertyBuilder<Domain.ValueObjects.File> builder)
    {
        builder.Property(e => e.OriginalName).HasMaxLength(100).HasComment("Original full file name, with extension");
        builder.Property(e => e.FileServerId).HasMaxLength(100);
        builder.Property(e => e.Size).HasComment("size of the file");

        builder.ComplexProperty(
            o => o.Extension, complexBuilder =>
            {
                complexBuilder.Property(n => n.Value)
                .HasColumnName("Value");
            }    
        );

        builder.ComplexProperty(
            o => o.Name, complexBuilder =>
            {
                complexBuilder.Property(n => n.Value)
                .HasColumnName("Value");
            }
        );
    }
}
