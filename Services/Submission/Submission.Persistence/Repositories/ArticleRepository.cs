using Blocks.EntityFramework;
using Blocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.Repositories;

public class ArticleRepository(SubmissionDbContext context)
    : Repository<SubmissionDbContext, Article>(context)
{
    public async Task<Article> GetByIdOrThrowAsync(int id, CancellationToken cancellationToken = default)
    {
        var article = await Entities
            .Include(article => article.Actors)
            .ThenInclude(actor => actor.Person)
            .SingleOrDefaultAsync(article => article.Id == id, cancellationToken);

        return article ?? throw new NotFoundException($"Article with id {id} was not found.");
    }

    public async Task<Author> GetAuthorByIdOrThrowAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await Context.Authors.FindAsync([id], cancellationToken);
        return author ?? throw new NotFoundException($"Author with id {id} was not found.");
    }

    public async Task<Author> GetAuthorByUserIdOrThrowAsync(int userId, CancellationToken cancellationToken = default)
    {
        var author = await Context.Authors
            .SingleOrDefaultAsync(author => author.UserId == userId, cancellationToken);

        return author ?? throw new NotFoundException($"No author profile is linked to user {userId}.");
    }
}
