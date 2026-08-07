namespace batteries.Extensions
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan AtLeast(this TimeSpan timeSpan, TimeSpan minimalTimeSpan)
        {
            if (timeSpan < minimalTimeSpan) return minimalTimeSpan;
            return timeSpan;
        }
    }
}
