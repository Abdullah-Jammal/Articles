using Auth.Domain.Users;
using Blocks.EntityFramework;
using Blocks.EntityFramework.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigrations;

internal class UserEntityConfigrations : EntityConfigration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.OwnsOne(u => u.Person, person =>
        {
            person.Property(p => p.FirstName).IsRequired().HasMaxLength(50).HasColumnName("FirstName");
            person.Property(p => p.LastName).IsRequired().HasMaxLength(50).HasColumnName("LastName");
            person.Property(p => p.Gender).IsRequired().HasEnumConversion().HasColumnName("Gender");
            person.Ignore(p => p.FullName);

            person.OwnsOne(p => p.Honorific, honorific =>
            {
                honorific.Property(h => h.Value).HasMaxLength(20).HasColumnName("Honorific");
                honorific.WithOwner();
            });

            person.OwnsOne(p => p.ProfessionalProfile, profile =>
            {
                profile.Property(p => p.Position).HasMaxLength(100).HasColumnNameSameAsProperty();
                profile.Property(p => p.CompanyName).HasMaxLength(100).HasColumnNameSameAsProperty();
                profile.Property(p => p.Affiliation).HasMaxLength(100).HasColumnNameSameAsProperty();
                profile.WithOwner();
            });

            person.Property(p => p.PictureUrl).HasMaxLength(200).HasColumnName("PictureUrl");
            person.WithOwner();
        });
        builder.Navigation(u => u.Person).IsRequired();

        builder.HasMany(u => u.UserRoles)
               .WithOne().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
               .WithOne().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
