using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Submission.Application.Features.UploadFile.UploadManuscriptFile;

public record UploadManuscriptFileCommand : ArticleCommand
{
    [Required]
    public AssetType AssetType { get; init; }
    [Required]
    public IFormFile File { get; init; } = null!;

    public override ArticleActionType ActionType => ArticleActionType.Upload;
}

public class UploadManuscriptFileCommandValidator : ArticleCommandValidator<UploadManuscriptFileCommand>    
{
    public UploadManuscriptFileCommandValidator()
    {
        RuleFor(x => x.AssetType)
            .IsInEnum()
            .WithMessage("Invalid asset type. Allowed values are: Manuscript, SupplementaryFile, Figure, DaraShet.");
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(file => file.Length > 0)
            .WithMessage("File cannot be empty.");
    }
}
