using Auth.Domain.Users.Events;
using Blocks.AspNetCore;
using EmailService.Contracts;
using FastEndpoints;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net;

namespace Auth.API.Features.Users.CreateUser;

public class SendConfirmationEmailOnUserCreatedHandler
    (IEmailService emailService, IOptions<EmailOptions> emailOptions, IHttpContextAccessor httpContextAccessor)
    : IEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent eventModel, CancellationToken ct)
    {
        var url = httpContextAccessor.HttpContext?.Request.BaseUrl()
            .AppendPathSegment("password")
            .SetQueryParams(new { eventModel.ResetPasswordToken });
        var emailMessage = BuildConfirmationEmail(eventModel.user, url ?? string.Empty, emailOptions.Value.EmailFromAddress);
        await emailService.SendEmailAsync(emailMessage, ct);
    }

    public EmailMessage BuildConfirmationEmail(User user, string resetLink, string fromEmailAddress)
    {
        var email = user.Email ?? throw new InvalidOperationException("Cannot send a confirmation email to a user without an email address.");
        var content = $"<p>Hello {WebUtility.HtmlEncode(user.Person.FullName)},</p>" +
            $"<p>Welcome! <a href=\"{WebUtility.HtmlEncode(resetLink)}\">Set your password</a> to get started.</p>";

        return new EmailMessage(
            "Welcome to Our Service! Please Confirm Your Email Address",
            new Content(ContentType.Html, content),
            new EmailAddress("Our Service Team", fromEmailAddress),
            new List<EmailAddress> { new EmailAddress(user.Person.FullName, email) }
            );
    }
}
