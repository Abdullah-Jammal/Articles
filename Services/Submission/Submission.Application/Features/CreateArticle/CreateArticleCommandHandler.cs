using Blocks.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Blocks.Exceptions;

namespace Submission.Application.Features.CreateArticle;

internal class CreateArticleCommandHandler(Repository<Journal> journalRepository) 
    : IRequestHandler<CreateArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateArticleCommand command, CancellationToken ct)
    {
        var journal = await journalRepository.FindByIdOrThrowAsync(command.JournalId, ct);
        var article = journal.CreateArticle(command.Title, command.ArticleType, command.Scope);
        await AssignCurrentUserAsAuthor(article, command, ct);
        await journalRepository.SaveChangesAsync(ct);
        return new IdResponse(article.Id);
    }

    private async Task AssignCurrentUserAsAuthor(
        Article article,
        CreateArticleCommand command,
        CancellationToken cancellationToken)
    {
        var author = await journalRepository.Context.Authors
            .SingleOrDefaultAsync(a => a.UserId == command.CreatedById, cancellationToken);
        if (author is null)
            throw new NotFoundException($"No author profile is linked to user {command.CreatedById}.");

        article.AssignAuthor(author, [ContributionArea.OriginalDraft], isCorrespondingAuthor: true);
    }
}
