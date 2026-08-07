
using Apache.NMS;

namespace batteries.Apache.NMS.Interfaces
{
    /// <summary>
    /// Provides a high-level API for sending messages, receiving messages as reactive streams, and performing request-response patterns over Apache.NMS.
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Sends a simple string message to a destination.
        /// </summary>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The message body to send.</param>
        /// <returns>A task that completes when the message has been sent.</returns>
        Task Send(string destination, string message);

        /// <summary>
        /// Sends a string message with optional properties (headers).
        /// </summary>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The message body to send.</param>
        /// <param name="properties">Optional message properties/headers (can be null).</param>
        /// <returns>A task that completes when the message has been sent.</returns>
        Task Send(string destination, string message, IDictionary<string, string>? properties);

        /// <summary>
        /// Sends a typed message object.
        /// </summary>
        /// <typeparam name="T">The message type, must implement IMessage and be a class.</typeparam>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The message object to send.</param>
        /// <returns>A task that completes when the message has been sent.</returns>
        Task Send<T>(string destination, T message) where T : class, IMessage;

        /// <summary>
        /// Sends a typed message using a specific NMS session.
        /// </summary>
        /// <typeparam name="T">The message type, must implement IMessage and be a class.</typeparam>
        /// <param name="session">The Apache.NMS session to use for sending.</param>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The message object to send.</param>
        /// <returns>A task that completes when the message has been sent.</returns>
        Task Send<T>(ISession session, string destination, T message) where T : class, IMessage;

        /// <summary>
        /// Performs a synchronous request-response pattern with string messages.
        /// </summary>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The request message body.</param>
        /// <param name="useTempDestination">If true, creates a temporary reply destination; if false, uses replyDestination.</param>
        /// <param name="replyDestination">The destination to listen for the reply (used if useTempDestination is false).</param>
        /// <returns>A task that completes with the reply message, or null if no reply is received.</returns>
        Task<string?> Request(string destination, string message, bool useTempDestination, string replyDestination);

        /// <summary>
        /// Performs a request-response with optional message properties.
        /// </summary>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The request message body.</param>
        /// <param name="properties">Optional message properties/headers.</param>
        /// <param name="useTempDestination">If true, creates a temporary reply destination; if false, uses replyDestination.</param>
        /// <param name="replyDestination">The destination to listen for the reply.</param>
        /// <returns>A task that completes with the reply message, or null if no reply is received.</returns>
        Task<string?> Request(string destination, string message, IDictionary<string, string>? properties, bool useTempDestination, string replyDestination);

        /// <summary>
        /// Performs a typed request-response pattern.
        /// </summary>
        /// <typeparam name="TIn">The request message type, must implement IMessage and be a class.</typeparam>
        /// <typeparam name="TOut">The response message type, must implement IMessage and be a class.</typeparam>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The request message object.</param>
        /// <param name="useTempDestination">If true, creates a temporary reply destination; if false, uses replyDestination.</param>
        /// <param name="replyDestination">The destination to listen for the reply.</param>
        /// <returns>A task that completes with the reply message, or null if no reply is received.</returns>
        Task<TOut?> Request<TIn, TOut>(string destination, TIn message, bool useTempDestination, string replyDestination) where TIn : class, IMessage where TOut : class, IMessage;

        /// <summary>
        /// Performs a typed request-response using a specific NMS session.
        /// </summary>
        /// <typeparam name="TIn">The request message type, must implement IMessage and be a class.</typeparam>
        /// <typeparam name="TOut">The response message type, must implement IMessage and be a class.</typeparam>
        /// <param name="session">The Apache.NMS session to use.</param>
        /// <param name="destination">The target queue or topic URI.</param>
        /// <param name="message">The request message object.</param>
        /// <param name="useTempDestination">If true, creates a temporary reply destination; if false, uses replyDestination.</param>
        /// <param name="replyDestination">The destination to listen for the reply.</param>
        /// <returns>A task that completes with the reply message, or null if no reply is received.</returns>
        Task<TOut?> Request<TIn, TOut>(ISession session, string destination, TIn message, bool useTempDestination, string replyDestination) where TIn : class, IMessage where TOut : class, IMessage;

        /// <summary>
        /// Consumes messages from a destination as a reactive stream.
        /// </summary>
        /// <typeparam name="T">The message type, must implement IMessage and be a class.</typeparam>
        /// <param name="destination">The queue or topic URI to consume from.</param>
        /// <returns>An observable stream of messages.</returns>
        IObservable<T?> Consume<T>(string destination) where T : class, IMessage;

        /// <summary>
        /// Consumes messages from a destination with a JMS selector filter.
        /// </summary>
        /// <typeparam name="T">The message type, must implement IMessage and be a class.</typeparam>
        /// <param name="destination">The queue or topic URI to consume from.</param>
        /// <param name="selector">A JMS selector string to filter messages (e.g., "Priority > 5").</param>
        /// <returns>A filtered stream of messages matching the selector.</returns>
        IObservable<T?> Consume<T>(string destination, string selector) where T : class, IMessage;

        /// <summary>
        /// Creates a text message with optional properties.
        /// </summary>
        /// <param name="session">The Apache.NMS session to use for message creation.</param>
        /// <param name="message">The message body.</param>
        /// <param name="properties">Optional message properties/headers.</param>
        /// <returns>A new text message ready to be sent.</returns>
        ITextMessage CreateTextMessage(ISession session, string message, IDictionary<string, string>? properties);

        /// <summary>
        /// Gets a value indicating whether the message bus is currently running.
        /// </summary>
        /// <value>true if the bus is connected and running; false if the bus is stopped or not connected.</value>
        bool IsRunning { get; }

        /// <summary>
        /// An observable that emits connection state changes.
        /// </summary>
        /// <value>An observable that emits true when connected to the message broker, false when disconnected.</value>
        IObservable<bool> Connected { get; }

        /// <summary>
        /// Gets the underlying Apache.NMS connection.
        /// </summary>
        /// <value>The active NMS connection used by this message bus.</value>
        IConnection Connection { get; }

        /// <summary>
        /// Creates a new Apache.NMS session.
        /// </summary>
        /// <returns>A new NMS session that can be used for sending or receiving messages.</returns>
        ISession CreateSession();
    }
}