namespace batteries.Apache.NMS.Interfaces
{
    /// <summary>
    /// Manages multiple message bus instances and provides access to configured destinations.
    /// </summary>
    public interface IBusManager
    {
        /// <summary>
        /// Retrieves a message bus instance by its registered name.
        /// </summary>
        /// <param name="name">The name of the message bus to retrieve.</param>
        /// <returns>The message bus instance associated with the given name.</returns>
        IMessageBus GetMessageBusByName(string name);

        /// <summary>
        /// Retrieves the destination address (queue or topic URI) associated with a given name.
        /// </summary>
        /// <param name="name">The logical name of the destination.</param>
        /// <returns>The physical destination address (e.g., "queue://my-queue" or "topic://my-topic").</returns>
        string GetDestinationByName(string name);

        /// <summary>
        /// Gets the current state of all managed message buses.
        /// </summary>
        /// <value>A dictionary mapping bus names to their running state (true = running, false = stopped).</value>
        Dictionary<string, bool> States { get; }

        /// <summary>
        /// An observable that emits when the bus manager has completed initialization.
        /// </summary>
        /// <value>An observable that emits true when initialization is complete, false if it fails.</value>
        IObservable<bool> Initialized { get; }
    }
}