using Auth.Domain.Role;
using Auth.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence;

public class AuthDBContext(DbContextOptions<AuthDBContext> options)
   : IdentityDbContext<User, Role, int>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuthDBContext).Assembly);
    }
}
