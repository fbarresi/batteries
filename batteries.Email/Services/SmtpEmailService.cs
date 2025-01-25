using batteries.Email.Interfaces;
using batteries.Email.Settings;
using MailKit.Net.Smtp;
using MimeKit;

namespace batteries.Email.Services;

public class SmtpEmailService : IEmailSender
{
    private readonly SmtpSettings settings;

    public SmtpEmailService(SmtpSettings settings)
    {
        this.settings = settings;
    }
    
    public async Task SendEmailAsync(string recipient, string subject, string htmlMessage)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress (settings.SenderName, settings.SenderAddress));
        message.To.Add (new MailboxAddress (recipient, recipient));
        message.Subject = subject;

        message.Body = new TextPart ("html")
        {
            Text = htmlMessage 
        };

        using var client = new SmtpClient ();
        await client.ConnectAsync(settings.SmtpServer, settings.Port, settings.UseSsl);

        // Note: only needed if the SMTP server requires authentication
        if(!string.IsNullOrEmpty(settings.Password))
            await client.AuthenticateAsync (settings.Username, settings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync (true);
    }
}
