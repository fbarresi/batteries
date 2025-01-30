
using Apache.NMS;

namespace batteries.Apache.NMS.Interfaces
{
    public interface IMessageBus
    {
        Task Send(string destination, string message);
        Task<string> Request(string destination, string message, bool useTempDestination, string replyDestination);
        Task<string> Request(string destination, string message, IDictionary<string, string>? properties, bool useTempDestination, string replyDestination);
        IObservable<T?> Consume<T>(string destination) where T : class, IMessage;
        bool IsRunning { get; }
        IObservable<bool> Connected { get; }
        IObservable<T?> Consume<T>(string destination, string selector) where T : class, IMessage;
        Task Send(string destination, string message, IDictionary<string, string>? properties);
    }
}