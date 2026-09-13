namespace Blocks.Core.FluentValidation;

public static class ValidationMessage
{
    public const string InvalidId = "The {0} must be greater than 0.";
    public const string MaxLengthExceeded = "The {0} must not exceed {1} characters.";
    public const string NullOrEmptyValue = "The {0} must not be null or empty.";
}