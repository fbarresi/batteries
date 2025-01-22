namespace batteries.Apache.NMS.Settings
{
    public class MessageBusSessionSettings
    {
        public string ServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DefaultDestination { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Persistent = 0 ; NonPersistent = 1
        /// </summary>
        public int DeliveryMode { get; set; } = 1;
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}