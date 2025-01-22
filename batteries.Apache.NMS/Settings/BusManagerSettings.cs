namespace batteries.Apache.NMS.Settings
{
    public class BusManagerSettings
    {
        public List<MessageBusSessionSettings> BusSettings { get; set; }
        public Dictionary<string, string> Destinations { get; set; }
    }
}