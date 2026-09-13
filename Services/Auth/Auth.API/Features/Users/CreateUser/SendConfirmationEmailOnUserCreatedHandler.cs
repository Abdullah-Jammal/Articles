using Auth.Domain.Users.Events;
using Blocks.AspNetCore;
using EmailService.Contracts;
using FastEndpoints;
using Flurl;
using Microsoft.Extensions.Options;

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
        const string ConfirmationEmail = "Welcome to Our Service! Please Confirm Your Email Address";

        return new EmailMessage(
            "Welcome to Our Service! Please Confirm Your Email Address",
            new Content(ContentType.Html, string.Format(ConfirmationEmail, user.FullName, resetLink)),
            new EmailAddress(fromEmailAddress, "Our Service Team"),
            new List<EmailAddress> { new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}") }
            );
    }
}
