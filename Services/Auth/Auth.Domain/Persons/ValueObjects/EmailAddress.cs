using Blocks.Core;
using Blocks.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace Auth.Domain.Persons.ValueObjects;

public class EmailAddress : StringValueObject
{
    private EmailAddress(string value) 
    { 
        Value = value;
        NormalizedEmail = Value.ToUpperInvariant();
    }

    public string NormalizedEmail { get; internal set; }
    public static EmailAddress Create(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        Guard.ThrowIfFalse(IsValidEmail(value), "Invalid email address format.");

        return new EmailAddress(value);
    }

    private static bool IsValidEmail(string email)
    {
        const string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    public static implicit operator EmailAddress(string value)
    {
        return Create(value);
    }

    public static implicit operator string(EmailAddress email)
    {
        return email.Value;
    }

    public override int GetHashCode() => NormalizedEmail.GetHashCode();
}
