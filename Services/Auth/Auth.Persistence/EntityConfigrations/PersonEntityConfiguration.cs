using Auth.Domain.Persons;
using Blocks.EntityFrameworkCore.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blocks.EntityFrameworkCore;
using Auth.Domain.Persons.ValueObjects;

namespace Auth.Persistence.EntityConfigrations;

internal class PersonEntityConfiguration : EntityConfigration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Gender).IsRequired().HasEnumConversion();

        builder.OwnsOne(
            e => e.Email, b =>
            {
                b.Property(n => n.Value).HasMaxLength(50)
                .HasColumnName(nameof(Person.Email));
                b.Property(n => n.NormalizedEmail)
                .HasMaxLength(50)
                .HasColumnName(nameof(EmailAddress.NormalizedEmail));
                b.HasIndex(e => e.NormalizedEmail).IsUnique();
            }
        );

        builder.OwnsOne(
            e => e.Honorific, b =>
            {
                b.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName(nameof(Person.Honorific));

                b.WithOwner();
            }    
        );
        builder.OwnsOne(p => p.ProfessionalProfile, profile =>
        {
            profile.Property(p => p.Position).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.Property(p => p.CompanyName).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.Property(p => p.Affiliation).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.WithOwner();
        });
    }
}
