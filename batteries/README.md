# batteries ![logo](https://raw.githubusercontent.com/gammasoft/fatcow/refs/heads/master/32x32/battery_charge.png)

[![NuGet Version](https://img.shields.io/nuget/vpre/batteries)](https://www.nuget.org/packages/batteries/)

This is a personal and opinionated collection of packages with useful code.

Sometimes you've got a new game to play and you realize: no batteries included 😩
<br/>From now on, you just need to use batteries!


## Description

This package contains useful common extension methods for everyday use.

## Extensions

### Disposable
- AddDisposableTo : Add a disposable to a composite disposable without any extra statement

### Observable

missing extensions in `system.reactive`

- DisposeMany : similar to SelectMany, append disposables to a serial disposable
- LogAndRetry : better than retry, a retry extension that logs
- LogAndRetryAfterDelay : even better than retry and log, a retry that log and wait a bit afterwards
- RepeatAfterDelay : enhanced repeat extension
- RetryAfterDelay : enhanced retry extension
- RedoAfterDelay : enhanced redo extension

### String
- TrimAfter : trim everything after a special character

### TimeSpan

- AtLeas : sometimes you simply want to make sure nobody attempt to pass `TimeSpan.Zero`

### Url

- IsValidWebUrl : Copied from [stackoverflow](https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url)

### Xml

- ExtractValueFromXmlQuery : simply extractor with xml document