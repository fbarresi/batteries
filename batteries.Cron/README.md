# batteries.Cron ![logo](https://raw.githubusercontent.com/gammasoft/fatcow/refs/heads/master/32x32/battery_charge.png)

[![NuGet Version](https://img.shields.io/nuget/vpre/batteries.Cron)](https://www.nuget.org/packages/batteries.Cron)

## Description

This package contains a useful extension for scheduling observable like crontab that produces a series like `Observable.Interval`.

## Usage

Here is how to use it:

 ````csharp
var subscription = Observable.Cron("*/5 * * * *", Scheduler.CurrentThread)
            .Do(_ => Console.WriteLine("triggering timebased execution!"))
            .Subscribe();
````

Check the Cron syntax with this [parser](https://elmah.io/tools/cron-parser/#*/5_*_*_*_*).
