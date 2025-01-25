# batteries.Apache.NMS ![logo](https://raw.githubusercontent.com/gammasoft/fatcow/refs/heads/master/32x32/battery_charge.png)

[![NuGet Version](https://img.shields.io/nuget/vpre/batteries.Apache.NMS)](https://www.nuget.org/packages/batteries.Apache.NMS)

## Description

This package contains useful implementations of message bus services based on apache NMS.

## Usage

### With Message Bus Manager
The message bus manager can be used for managing multiple instances of message bus  on different locations.

Here is how to use it:

1. Add the setting binding
    ````csharp
   builder.Services.AddOptions<BusManagerSettings>()
    .BindConfiguration("BusManagerSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();
    builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BusManagerSettings>>().Value);

   ````
2. Adapt your `appsettings.json`
   and include the necessary settings:
   ````json
   "BusManagerSettings":{ 
    "BusSettings":
    [
      {
        "Name": "test",
        "ServerUrl":"activemq:tcp://localhost:61616?retryInterval=1000&retryIntervalMultiplier=1.5&maxRetryInterval=60000&reconnectAttempts=1000",
        "Username": "artemis",
        "Password": "artemis",
        "DefaultDestination": "topic://fancytopic"
      }
    ],
    "Destinations": {
      "abc": "queue://abc.xyz"
    }
   }
   ````
3. Add the background service
    ````csharp
    builder.Services.AddSingleton<MessageBusManagerService>();
    builder.Services.AddSingleton<IHostedService, MessageBusManagerService>(serviceProvider => serviceProvider.GetService<MessageBusManagerService>());
    builder.Services.AddSingleton<IBusManager, MessageBusManagerService>(serviceProvider => serviceProvider.GetService<MessageBusManagerService>());
    ````

4. Inject the service (for example in one controller)
    ```csharp
   public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IBusManager busManager;

        public DataController(ILogger<DataController> logger, IBusManager busManager)
        {
            this.logger = logger;
            this.busManager = busManager;
        }
    }
   ```


### Without Message Bus Manager

The usage without the manager has very similar steps. Unter theese condition the software will ony have a single instance of the message bus.

Here is how to use it:

1. Add the setting binding
    ````csharp
   builder.Services.AddOptions<MessageBusSessionSettings>()
    .BindConfiguration("MessageBusSessionSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();
    builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<MessageBusSessionSettings>>().Value);

   ````
2. Adapt your `appsettings.json`
   and include the necessary settings:
   ````json
   "MessageBusSessionSettings":
      {
        "Name": "test",
        "ServerUrl":"activemq:tcp://localhost:61616?retryInterval=1000&retryIntervalMultiplier=1.5&maxRetryInterval=60000&reconnectAttempts=1000",
        "Username": "artemis",
        "Password": "artemis",
        "DefaultDestination": "topic://fancytopic"
      }
   
   ````
3. Add the background service
    ````csharp
    builder.Services.AddSingleton<MessageBus>();
    builder.Services.AddSingleton<IHostedService, MessageBus>(serviceProvider => serviceProvider.GetService<MessageBus>());
    builder.Services.AddSingleton<IMessageBus, MessageBus>(serviceProvider => serviceProvider.GetService<MessageBus>());
    ````

4. Inject the service (for example in one controller)
    ```csharp
   public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IMessageBus messageBus;

        public DataController(ILogger<DataController> logger, IMessageBus messageBus)
        {
            this.logger = logger;
            this.messageBus = messageBus;
        }
    }
   ```
