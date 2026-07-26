namespace Submission.Domain.ValueObjects;

public class FileExtensions
{
    public IReadOnlyList<string> Extensions { get; init; } = null!;

    public bool IsValidExtension(string extension)
        => !Extensions.Any() || Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
}
