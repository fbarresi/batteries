using System.Reactive;

namespace batteries.Apache.NMS.Interfaces
{
    public interface IBusManager
    {
        IMessageBus GetMessageBusByName(string name);
        string GetDestinationByName(string name);
        Dictionary<string, bool> States { get; }
        IObservable<bool> Initialized { get; }
    }
}