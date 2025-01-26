// See https://aka.ms/new-console-template for more information

using Azure.Identity;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using MimeKit;

Console.WriteLine("Hello, World!");

var sender = Environment.GetEnvironmentVariable("Sender");
var recipient = Environment.GetEnvironmentVariable("Recipient");
var clientId = Environment.GetEnvironmentVariable("ClientId");
var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
var tenantId = Environment.GetEnvironmentVariable("TenantId");

var message = new MimeMessage ();
message.From.Add (new MailboxAddress (sender, sender));
message.To.Add (new MailboxAddress (recipient, recipient));
message.Subject = "test";

message.Body = new TextPart ("html")
{
    Text = "test message" 
};


var result = await GetConfidentialClientOAuth2CredentialsAsync ("SMTP", clientId, tenantId, clientSecret);
var oauth2 = new SaslMechanismOAuth2 (sender, result.AccessToken);

using var client = new SmtpClient ();
await client.ConnectAsync ("smtp.office365.com", 587, SecureSocketOptions.StartTls);
await client.AuthenticateAsync (oauth2);
await client.SendAsync(message);
await client.DisconnectAsync (true);


Console.WriteLine("Done!");


static async Task<AuthenticationResult> GetConfidentialClientOAuth2CredentialsAsync (string protocol, string? clientId, string? tenantId, string? clientSecret, CancellationToken cancellationToken = default)
{
    var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create (clientId)
        .WithAuthority ($"https://login.microsoftonline.com/{tenantId}/v2.0")
        .WithClientSecret (clientSecret)
        .Build ();

    string[] scopes;

    if (protocol.Equals ("SMTP", StringComparison.OrdinalIgnoreCase)) {
        scopes = new string[] {
            // For SMTP, use the following scope
            "https://outlook.office365.com/.default"
        };
    } else {
        scopes = new string[] {
            // For IMAP and POP3, use the following scope
            "https://ps.outlook.com/.default"
        };
    }

    return await confidentialClientApplication.AcquireTokenForClient (scopes).ExecuteAsync (cancellationToken);
}

// Common Code

