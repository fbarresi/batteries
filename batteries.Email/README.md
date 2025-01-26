# batteries.Email ![logo](https://raw.githubusercontent.com/gammasoft/fatcow/refs/heads/master/32x32/battery_charge.png)

[![NuGet Version](https://img.shields.io/nuget/vpre/batteries.Email)](https://www.nuget.org/packages/batteries.Email)

## Description

This package contains a useful email service based on [MailKit](https://github.com/jstedfast/MailKit).

## Usage

The message bus manager can be used for managing multiple instances of message bus  on different locations.

Here is how to use it:

1. Add the setting binding
    ````csharp
   builder.Services.AddOptions<SmtpSettings>()
    .BindConfiguration("MailSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();
    builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SmtpSettings>>().Value);

   ````
2. Adapt your `appsettings.json`
   and include the necessary settings: <small>(remember some provider like gmail enforce the usage of [app passwords](https://knowledge.workspace.google.com/kb/how-to-create-app-passwords-000009237?hl=en))</small>
   ````json
   "MailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 465,
    "Username": "my-fancy-gmail-account@gmail.com",
    "Password": "xxxx xxxx xxxx xxxx",
   "SenderAddress": "no-reply@no-mail.com",
   "SenderName": "Fancy Service"
   }
   ````
3. Add the background service
    ````csharp
    builder.Services.AddSingleton<SmtpEmailService>();
    builder.Services.AddSingleton<IHostedService, SmtpEmailService>(serviceProvider => serviceProvider.GetService<SmtpEmailService>());
    builder.Services.AddSingleton<IEmailSender, SmtpEmailService>(serviceProvider => serviceProvider.GetService<SmtpEmailService>());
    ````

4. Inject the service (for example in one controller)
    ```csharp
   public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IEmailSender sender;

        public DataController(ILogger<DataController> logger, IEmailSender sender)
        {
            this.logger = logger;
            this.sender = sender;
        }
    }
   ```


### OAuth2 (and MS Office 356)

#### Preparation:
Follow [this guide](https://github.com/jstedfast/MailKit/blob/master/ExchangeOAuth2.md#web-services) for granting mailbox permission to a serice principal.

`todo`
