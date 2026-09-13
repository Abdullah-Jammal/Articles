using Auth.Domain.Persons;
using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence.Repositories;

public class PersonRepository(AuthDBContext dbContext) : Repository<AuthDBContext,Person>(dbContext)
{
    protected override IQueryable<Person> Query() 
        => base.Query().Include(p => p.User);

    public async Task<Person?> GetByUserIdAsync(int userId, CancellationToken ct = default)
    =>
        await Query()
        .SingleOrDefaultAsync(x => x.UserId == userId);

    public async Task<Person?> GetByEmailAsync(string email, CancellationToken ct = default)
    =>
        await Query()
        .SingleOrDefaultAsync(x => x.Email.NormalizedEmail == email.ToUpperInvariant());
}
