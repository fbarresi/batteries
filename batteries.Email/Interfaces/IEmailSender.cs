namespace batteries.Email.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string recipient, string subject, string htmlMessage);
}