using Auth.Domain.Role;
using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.EntityConfigrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigrations;

internal class RoleEntityConfigration : EntityConfigration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Type).IsRequired().HasEnumConversion();
        builder.Property(r => r.Description).IsRequired().HasMaxLength(500);
    }
}
