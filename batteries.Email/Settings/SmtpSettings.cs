using System.Text;

namespace batteries.Email.Settings;

public class SmtpSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool UseSsl { get; set; } = true;
    public string SenderAddress { get; set; }
    public string SenderName { get; set; }
}