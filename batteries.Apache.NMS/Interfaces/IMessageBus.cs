
using Apache.NMS;

namespace batteries.Apache.NMS.Interfaces
{
    public interface IMessageBus
    {
        Task Send(string destination, string message);
        Task Send(string destination, string message, IDictionary<string, string>? properties);
        Task Send<T>(string destination, T message) where T : class, IMessage;
        Task<string?> Request(string destination, string message, bool useTempDestination, string replyDestination);
        Task<string?> Request(string destination, string message, IDictionary<string, string>? properties, bool useTempDestination, string replyDestination);
        Task<TOut?> Request<TIn, TOut>(string destination, TIn message, bool useTempDestination, string replyDestination) where TIn : class, IMessage where TOut : class, IMessage;
        IObservable<T?> Consume<T>(string destination) where T : class, IMessage;
        IObservable<T?> Consume<T>(string destination, string selector) where T : class, IMessage;
        bool IsRunning { get; }
        IObservable<bool> Connected { get; }
    }
}