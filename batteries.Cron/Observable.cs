using System.Reactive.Concurrency;
using System.Reactive.Linq;
using NCrontab;

namespace batteries.Cron;

public static class Observable
{
    //copied from https://stackoverflow.com/questions/26597393/cron-observable-sequence
    
    public static IObservable<int> Cron(string cron, IScheduler scheduler)
    {
        var schedule = CrontabSchedule.Parse(cron);
        return System.Reactive.Linq.Observable.Generate(0, d => true, d => d + 1, d => d,
            d => new DateTimeOffset(schedule.GetNextOccurrence(scheduler.Now.UtcDateTime)), scheduler);
    }
    public static IObservable<int> Cron(string cron) => Cron(cron, Scheduler.Default);
}