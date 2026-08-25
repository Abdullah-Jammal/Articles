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
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Gender).IsRequired().HasEnumConversion();

        builder.OwnsOne(h => h.Honorific, honorific =>
        {
            honorific.Property(h => h.Value).HasMaxLength(20).HasColumnName(nameof(User.Honorific));
            honorific.WithOwner();
        });

        builder.OwnsOne(p => p.ProfessionalProfile, profile =>
        {
            profile.Property(p => p.Position).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.Property(p => p.CompanyName).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.Property(p => p.Affiliation).HasMaxLength(100).HasColumnNameSameAsProperty();
            profile.WithOwner();
        });

        builder.Property(u => u.PictureUrl).HasMaxLength(200);

        builder.HasMany(u => u.UserRoles)
               .WithOne().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
