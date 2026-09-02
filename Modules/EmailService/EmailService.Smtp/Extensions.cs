using EmailService.Contracts;
using MimeKit;

namespace EmailService.Smtp;

public static class Extensions
{
    public static MailboxAddress ToMailBoxAddress(this EmailAddress emailAddress)
    {
        return new MailboxAddress(emailAddress.Name, emailAddress.Address);
    }
    public static MimeMessage ToMailKitMessage(this EmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.Subject = emailMessage.Subject;
        message.From.Add(emailMessage.From.ToMailBoxAddress());
        message.To.AddRange(emailMessage.To.Select(r => r.ToMailBoxAddress()).ToList());
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailMessage.Content.Value
        };
        message.Body = bodyBuilder.ToMessageBody();
        return message;
    }
}
