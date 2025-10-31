using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Apache.NMS;
using Apache.NMS.Util;
using batteries.Apache.NMS.Interfaces;
using batteries.Apache.NMS.Settings;
using batteries.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace batteries.Apache.NMS.Services;

public class MessageBus : BackgroundService, IMessageBus, IDisposable
{
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            disposables.Dispose();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(-1, stoppingToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        reconnectSubject
            .StartWith(Unit.Default)
            .Throttle(TimeSpan.FromSeconds(3))
            .Do(_ => connectionSubject.OnNext(false))
            .Do(_ => CreateConnection())
            .LogAndRetryAfterDelay(logger, TimeSpan.FromSeconds(5), "Error while creating connection")
            .Subscribe()
            .AddDisposableTo(disposables);

        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        defaultProducer?.Close();
        session?.Close();
        connection?.Stop();

        return base.StopAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private readonly ILogger<MessageBus> logger;
    private readonly MessageBusSessionSettings settings;
    private IConnection connection;
    private ISession session;
    private IMessageProducer defaultProducer;
    private readonly Subject<Unit> reconnectSubject = new();
    private readonly CompositeDisposable disposables = new();
    private readonly BehaviorSubject<bool> connectionSubject = new(false);
    public bool IsRunning => connection?.IsStarted ?? false;
    public IObservable<bool> Connected => connectionSubject.AsObservable();
    
    public MessageBus(ILogger<MessageBus> logger, MessageBusSessionSettings settings)
    {
        this.logger = logger;
        this.settings = settings;
        reconnectSubject.AddDisposableTo(disposables);
        connectionSubject.AddDisposableTo(disposables);
    }
    
    private void CreateConnection()
    {
        logger.LogInformation("Starting message bus {Name}...", settings.Name);
        try
        {
            logger.LogInformation("Connect to server: {Server}", settings.ServerUrl);
            var factory = new NMSConnectionFactory(settings.ServerUrl);
            connection = factory.CreateConnection(settings.Username, settings.Password);
            connection.AddDisposableTo(disposables);
            session = connection.CreateSession();
            session.AddDisposableTo(disposables);

            CreateDefaultProducer();
            connection.Start();
            logger.LogInformation("Connection for message bus {Name} started!", settings.Name);
            connectionSubject.OnNext(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while starting message bus service");
            throw;
        }
    }

    private void CreateDefaultProducer()
    {
        if (!string.IsNullOrEmpty(settings.DefaultDestination))
        {
            var destination = SessionUtil.GetDestination(session, settings.DefaultDestination);
            logger.LogInformation("Destination: {Destination}", settings.DefaultDestination);

            defaultProducer = session.CreateProducer(destination);
            defaultProducer.DeliveryMode = (MsgDeliveryMode)settings.DeliveryMode;
            defaultProducer.RequestTimeout = settings.RequestTimeout;
            defaultProducer.AddDisposableTo(disposables);
        }
    }

    public Task Send(string destination, string message)
    {
        return Send(destination, message, null);
    }
    public Task Send(string destination, string message, IDictionary<string, string>? properties)
    {
        var request = CreateTextMessage(message, properties);
        return Send(destination, request);
    }

    public Task Send<T>(string destination, T message) where T : class, IMessage
    {
        try
        {
            if (destination.Equals(settings.DefaultDestination))
            {
                defaultProducer.Send(message);
            }
            else
            {
                using var messageProducer = GetProducer(destination);
                messageProducer.Send(message);
                messageProducer.Close();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while sending message to {Destination}", destination);
            reconnectSubject.OnNext(Unit.Default);
            throw;
        }
        return Task.FromResult(true);
    }

    private ITextMessage CreateTextMessage(string message, IDictionary<string, string>? properties)
    {
        var request = session.CreateTextMessage(message);

        if (properties != null)
        {
            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    request.Properties[property.Key] = property.Value;
                }
            }
        }
        request.NMSMessageId = Guid.NewGuid().ToString();

        return request;
    }

    public Task<string?> Request(string destination, string message, bool useTempDestination, string replyDestination)
    {
        return Request(destination, message, null, useTempDestination, replyDestination);
    }
    public async Task<string?> Request(string destination, string message, IDictionary<string, string>? properties, bool useTempDestination, string replyDestination)
    {
        
        var request = CreateTextMessage(message, properties);
        var reply = await Request<ITextMessage, ITextMessage>(destination, request, useTempDestination, replyDestination);
        return reply?.Text;
    }

    public Task<TOut?> Request<TIn, TOut>(string destination, TIn message, bool useTempDestination, string replyDestination) where TIn : class, IMessage where TOut : class, IMessage
    {
        using var replyDest = GetDestination(useTempDestination, replyDestination);
        try
        {
            message.NMSReplyTo = replyDest;
            
            using var consumer = session.CreateConsumer(replyDest);
            
            logger.LogInformation("Sending message with ID: {MessageId} and ReplyTo: {ReplyTo}", message?.NMSMessageId, replyDest);
            
            if (destination.Equals(settings.DefaultDestination))
            {
                defaultProducer.Send(message);
            }
            else
            {
                using var messageProducer = GetProducer(destination);
                messageProducer.Send(message);
                messageProducer.Close();
            }
            
            var reply = consumer.Receive(settings.RequestTimeout);
            logger.LogInformation("Received message with ID: {MessageId}", reply?.NMSMessageId);
            var textReply = (reply as ITextMessage)?.Text;
            logger.LogInformation("Received message with text: {Reply}", textReply);
            return Task.FromResult(reply as TOut);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while sending JMS message");
            reconnectSubject.OnNext(Unit.Default);
            throw;
        }
        finally
        {
            if (replyDest.IsTemporary)
            {
                var temporaryQueue = (replyDest as ITemporaryQueue);
                temporaryQueue?.Delete();
                logger.LogInformation("temp destination {TempQueue} deleted", temporaryQueue?.QueueName);
            }
        }
    }

    public IObservable<T?> Consume<T>(string destination, string selector) where T : class, IMessage
    {
        return Observable.Create<T>(obs =>
        {
            var disposable = new CompositeDisposable();
            
            var dest = SessionUtil.GetDestination(session, destination);
            dest.AddDisposableTo(disposable);
            var consumer = session.CreateConsumer(dest, selector);
            consumer.AddDisposableTo(disposable);

            Observable.Return(Unit.Default)
                .SelectMany(_ => consumer.ReceiveAsync())
                .Select(m => (m as T))
                .Repeat()
                .Subscribe(obs.OnNext!, e =>
                {
                    reconnectSubject.OnNext(Unit.Default);
                    obs.OnError(e);
                }, obs.OnCompleted)
                .AddDisposableTo(disposables);
            
            return disposable;
        });
    }
    
    public IObservable<T?> Consume<T>(string destination) where T : class, IMessage
    {
        return Consume<T>(destination, string.Empty);
    }

    private IDestination GetDestination(bool useTemp, string destination)
    {
        if (useTemp)
        {
            return session.CreateTemporaryQueue();
        }
        return SessionUtil.GetDestination(session, destination);
    }
    private IMessageProducer GetProducer(string destination)
    {
        var dest = SessionUtil.GetDestination(session, destination);
        logger.LogInformation("Creating producer for: {Destination}", destination);

        var customProducer = session.CreateProducer(dest);
        customProducer.DeliveryMode = (MsgDeliveryMode)settings.DeliveryMode;
        customProducer.RequestTimeout = settings.RequestTimeout;
        return customProducer;
    }
    
}