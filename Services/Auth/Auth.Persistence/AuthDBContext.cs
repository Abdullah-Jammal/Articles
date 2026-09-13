using Auth.Domain.Role;
using Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence;

public class AuthDBContext(DbContextOptions<AuthDBContext> options)
   : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
       IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuthDBContext).Assembly);
    }
}
