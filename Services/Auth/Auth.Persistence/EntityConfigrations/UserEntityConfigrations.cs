using Auth.Domain.Users;
using Blocks.EntityFrameworkCore.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigrations;

internal class UserEntityConfigrations : EntityConfigration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasMany(u => u.UserRoles)
       .WithOne().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
               .WithOne().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Person).WithOne(p => p.User)
            .HasForeignKey<User>(u => u.PersonId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
