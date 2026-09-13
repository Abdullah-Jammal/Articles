using FluentValidation;

namespace Blocks.Core.FluentValidation;

public static class Extensions
{
    public static IRuleBuilderOptions<T, TProperty> WithMessageForInvalidId<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string propertyName)
        => rule.WithMessage(_ => string.Format(ValidationMessage.InvalidId, propertyName));

    public static IRuleBuilderOptions<T, TProperty> NotEmptyWithMessage<T, TProperty>(
        this IRuleBuilder<T, TProperty> rule,
        string propertyName)
        => rule.NotEmpty().WithMessage(string.Format(ValidationMessage.NullOrEmptyValue, propertyName));

    public static IRuleBuilderOptions<T, string?> MaximumLengthWithMessage<T>(
        this IRuleBuilder<T, string?> rule,
        int maximumLength,
        string propertyName)
        => rule.MaximumLength(maximumLength)
            .WithMessage(string.Format(ValidationMessage.MaxLengthExceeded, propertyName, maximumLength));
}