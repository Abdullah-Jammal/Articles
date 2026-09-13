using Blocks.EntityFrameworkCore.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigrations;

internal class PersonEntityConfiguration : EntityConfigration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.HasDiscriminator(p => p.TypeDiscriminator)
            .HasValue<Person>(nameof(Person))
            .HasValue<Author>(nameof(Author));

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.Title)
            .HasMaxLength(255);
        builder.Property(p => p.Affiliation)
            .IsRequired()
            .HasMaxLength(512).HasComment("The affiliation of the person, e.g., the organization or institution they are associated with.");

        builder.ComplexProperty(p => p.EmailAddress, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("EmailAddress");
        });
    }
}
