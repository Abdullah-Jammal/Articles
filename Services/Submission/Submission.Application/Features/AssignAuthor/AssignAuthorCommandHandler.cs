namespace Submission.Application.Features.AssignAuthor;

public class AssignAuthorCommandHandler(ArticleRepository articleRepository)
    : IRequestHandler<AssignAuthorCommand, IdResponse>
{
    public async Task<IdResponse> Handle(AssignAuthorCommand command, CancellationToken cancellationToken)
    {
        var article = await articleRepository.GetByIdOrThrowAsync(command.ArticleId, cancellationToken);
        var author = await articleRepository.GetAuthorByIdOrThrowAsync(command.AuthorId, cancellationToken);

        article.AssignAuthor(author, command.ContributionAreas, command.IsCorrespondingAuthor);
        await articleRepository.SaveChangesAsync(cancellationToken);
        return new IdResponse(article.Id);
    }
}
