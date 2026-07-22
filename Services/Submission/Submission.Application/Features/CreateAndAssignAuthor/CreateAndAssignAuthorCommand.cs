namespace Submission.Application.Features.CreateAndAssignAuthor;

public record CreateAndAssignAuthorCommand(int? UserId, string? FirstName, string? LastName, string? Email, string? Title,
    string? Affiliation, bool IsCorrespondingAuthor, HashSet<ContributionArea> ContributionAreas)
    : ArticleCommand
{
    public override ArticleActionType ActionType => ArticleActionType.AssignAuthor;
}

public class CreateAndAssignAuthorCommandValidator : AbstractValidator<CreateAndAssignAuthorCommand>
{
    public CreateAndAssignAuthorCommandValidator()
    {
        RuleFor(x => x.ArticleId).GreaterThan(0);
        RuleFor(x => x.UserId).GreaterThan(0).When(x => x.UserId.HasValue);
        RuleFor(x => x.FirstName).NotEmpty().When(x => !x.UserId.HasValue);
        RuleFor(x => x.LastName).NotEmpty().When(x => !x.UserId.HasValue);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().When(x => !x.UserId.HasValue);
        RuleFor(x => x.Affiliation).NotEmpty().When(x => !x.UserId.HasValue);
        RuleFor(x => x.ContributionAreas).NotEmptyWithMessage(nameof(CreateAndAssignAuthorCommand.ContributionAreas));
        RuleForEach(x => x.ContributionAreas).IsInEnum();
    }
}
