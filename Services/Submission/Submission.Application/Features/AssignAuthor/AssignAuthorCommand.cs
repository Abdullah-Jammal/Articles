namespace Submission.Application.Features.AssignAuthor;

public record AssignAuthorCommand(int AuthorId, bool IsCorrespondingAuthor, HashSet<ContributionArea> ContributionAreas)
    : ArticleCommand
{
    public override ArticleActionType ActionType => ArticleActionType.AssignAuthor;
}

public class AssignAuthorCommandValidator : ArticleCommandValidator<AssignAuthorCommand>
{
    public AssignAuthorCommandValidator()
    {
        RuleFor(x => x.ArticleId)
            .GreaterThan(0).WithMessageForInvalidId(nameof(AssignAuthorCommand.ArticleId));
        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessageForInvalidId(nameof(AssignAuthorCommand.AuthorId));
        RuleFor(x => x.ContributionAreas)
            .NotEmptyWithMessage(nameof(AssignAuthorCommand.ContributionAreas));
        RuleForEach(x => x.ContributionAreas)
            .IsInEnum();
    }
}
