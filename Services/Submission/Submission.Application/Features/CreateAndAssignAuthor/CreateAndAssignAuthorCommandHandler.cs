namespace Submission.Application.Features.CreateAndAssignAuthor;

public class CreateAndAssignAuthorCommandHandler(ArticleRepository articleRepository)
    : IRequestHandler<CreateAndAssignAuthorCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateAndAssignAuthorCommand command, CancellationToken ct)
    {
        var article = await articleRepository.GetByIdOrThrowAsync(command.ArticleId, ct);
        var author = command.UserId is null
            ? Author.Create(command.Email!, command.FirstName!, command.LastName!, command.Title, command.Affiliation!)
            : await articleRepository.GetAuthorByUserIdOrThrowAsync(command.UserId.Value, ct);

        article.AssignAuthor(author, command.ContributionAreas, command.IsCorrespondingAuthor);
        await articleRepository.SaveChangesAsync(ct);
        return new IdResponse(article.Id);
    }
}
