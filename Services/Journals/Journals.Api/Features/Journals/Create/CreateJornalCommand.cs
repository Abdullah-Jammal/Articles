using FastEndpoints;
using FluentValidation;

namespace Journals.Api.Features.Journals.Create;

public record CreateJornalCommand(string Name, string Abbreviation, string Description, string ISSN, int ChiefEditorId)
{
}

public class CreateJornalCommandValidator : Validator<CreateJornalCommand>
{
    public CreateJornalCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Abbreviation).NotEmpty().WithMessage("Abbreviation is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ISSN).NotEmpty().WithMessage("ISSN is required.");
        RuleFor(x => x.ChiefEditorId).GreaterThan(0).WithMessage("Chief Editor Id must be greater than 0.");
    }
}
