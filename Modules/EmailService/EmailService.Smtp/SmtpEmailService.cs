using EmailService.Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace EmailService.Smtp;

public class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    public SmtpEmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }
    public async Task<bool> SendEmailAsync(EmailMessage emailMessage, CancellationToken ct = default)
    {
        var message = emailMessage.ToMailKitMessage();

        using var stmpClient = new SmtpClient();

        try
        {
            await stmpClient.ConnectAsync(_emailOptions.Smtp.Host, _emailOptions.Smtp.Port, _emailOptions.Smtp.UseSsl, ct);
            await stmpClient.AuthenticateAsync(_emailOptions.Smtp.UserName, _emailOptions.Smtp.Password, ct);
            await stmpClient.SendAsync(message);
        }
        catch(Exception)
        {
            // to do: log error
            return false;
        }

        return true;
    }
}
