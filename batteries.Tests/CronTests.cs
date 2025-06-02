using System.Reactive.Concurrency;
using batteries.Cron;
using Microsoft.Reactive.Testing;

namespace batteries.Tests;

public class CronTests
{
    [Test]
    public void TestCronInterval()
    {
        var scheduler = new TestScheduler();
        var end = scheduler.Now.UtcDateTime.AddMinutes(10);
        const string cron = "*/5 * * * *";
        var i = 0;
        var seconds = 0;
        var sub = Observable.Cron(cron, scheduler).Subscribe(x => i++);
        while (i < 2)
        {
            seconds++;
            scheduler.AdvanceBy(TimeSpan.TicksPerSecond);
        }
        Assert.IsTrue(seconds == 600);
        Assert.AreEqual(end, scheduler.Now.UtcDateTime);
        Assert.AreEqual(i, 2);
        sub.Dispose();
    }
}

