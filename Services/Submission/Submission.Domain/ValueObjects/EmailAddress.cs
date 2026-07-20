using Blocks.Core;
using System.Text.RegularExpressions;

namespace Submission.Domain.ValueObjects;

public class EmailAddress
{
    public string Value { get; private set; }
    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email address format.", nameof(value));
        return new EmailAddress(value);
    }

    private static bool IsValidEmail(string email)
    {
        const string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    public override string ToString() => Value;
}
