using Articles.Abstractions;
using Articles.Abstractions.Enums;
using Blocks.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;
using Submission.Persistence.Repositories;

namespace Submission.Application.Features.CreateArticle;

internal class CreateArticleCommandHandler(Repository<Journal> journalRepository) : IRequestHandler<CreateArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateArticleCommand command, CancellationToken ct)
    {
        var journal = await journalRepository.FindByIdOrThrowAsync(command.JournalId);
        var article = journal.CreateArticle(command.Title, command.ArticleType, command.Scope);
        await AssignCurrentUserAsAuthor(article, command);
        await journalRepository.SaveChangesAsync(ct);
        return new IdResponse(article.Id);
    }

    private async Task AssignCurrentUserAsAuthor(Article article, CreateArticleCommand command)
    {
        var author = await journalRepository.Context.Authors.SingleOrDefaultAsync(a => a.UserId == command.CreatedById);
        if (author != null)
        {
            article.AssignAuthor(author, [ContributionArea.OriginalDraft], isCorrespondingAuthor : true);
        }
    }
}
