namespace Submission.Application.Features.CreateArticle;

public record CreateArticleCommand(int JournalId, string Title, string Scope, ArticleType ArticleType) : ArticleCommand
{
    public override ArticleActionType ActionType => ArticleActionType.Create;
}

public class CreateArticleCommandValidator : ArticleCommandValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmptyWithMessage(nameof(CreateArticleCommand.Title))
            .MaximumLengthWithMessage(256, nameof(CreateArticleCommand.Title));
        RuleFor(x => x.Scope)
            .NotEmptyWithMessage(nameof(CreateArticleCommand.Scope))
            .MaximumLengthWithMessage(2048, nameof(CreateArticleCommand.Scope));
        RuleFor(x => x.JournalId).GreaterThan(0).WithMessageForInvalidId(nameof(CreateArticleCommand.JournalId));
    }
}
