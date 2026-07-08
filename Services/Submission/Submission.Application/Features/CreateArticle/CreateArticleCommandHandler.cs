using Articles.Abstractions;
using MediatR;
using Submission.Domain.Entities;
using Submission.Persistence.Repositories;

namespace Submission.Application.Features.CreateArticle;

internal class CreateArticleCommandHandler(Repository<Journal> journalRepository) : IRequestHandler<CreateArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateArticleCommand command, CancellationToken ct)
    {
        var journal = await journalRepository.FindByIdAsync(command.JournalId);
        if (journal == null)
        {
            throw new ArgumentException($"Journal with id {command.JournalId} not found");
        }
        var article = journal.CreateArticle(command.Title, command.ArticleType, command.Scope);
        await journalRepository.SaveChangesAsync(ct);
        return new IdResponse(article.Id);
    }
}
