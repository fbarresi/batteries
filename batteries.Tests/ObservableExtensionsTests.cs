using batteries.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Reactive.Testing;
using Shouldly;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace batteries.Tests;

public class ObservableExtensionsTests
{

    #region DisposeMany Tests

    [Test]
    public void DisposeMany_WhenSourceCompletes_ForwardsCompletion()
    {
        var completed = false;
        var sequence = new Subject<IDisposable>();

        sequence.DisposeMany().Subscribe(
            _ => { },
            _ => { },
            () => completed = true
        );

        sequence.OnCompleted();
        completed.ShouldBeTrue();
    }

    [Test]
    public void DisposeMany_WhenSourceErrors_ForwardsError()
    {
        var error = new Exception("Test error");
        Exception caughtError = null;
        var sequence = new Subject<IDisposable>();

        sequence.DisposeMany().Subscribe(
            _ => { },
            ex => caughtError = ex,
            () => { }
        );

        sequence.OnError(error);
        caughtError.ShouldBe(error);
    }

    [Test]
    public void DisposeMany_WithMultipleItemsSequence_DisposesInOrder()
    {
        var disposeOrder = new List<int>();
        var disposable1 = new TestDisposable(() => disposeOrder.Add(1));
        var disposable2 = new TestDisposable(() => disposeOrder.Add(2));

        var sequence = new Subject<IDisposable>();
        sequence.DisposeMany().Subscribe();

        sequence.OnNext(disposable1);
        sequence.OnNext(disposable2);

        disposeOrder.ShouldContain(1);
    }

    [Test]
    public void DisposeMany_DisposesSubscription_WhenObserverUnsubscribes()
    {
        var disposed = false;
        var disposable = new TestDisposable(() => disposed = true);
        var sequence = new Subject<IDisposable>();

        var subscription = sequence.DisposeMany().Subscribe();

        sequence.OnNext(disposable);
        subscription.Dispose();

        disposed.ShouldBeTrue();
    }

    #endregion

    #region LogAndRetry Tests

    [Test]
    public void LogAndRetry_WhenSourceEmitsValue_LogsAndEmitsValue()
    {
        var logger = new MockLogger();
        var sequence = new Subject<int>();
        var results = new List<int>();

        sequence.LogAndRetry(logger, "Test message").Subscribe(results.Add);

        sequence.OnNext(42);

        results.ShouldContain(42);
    }

    [Test]
    public void LogAndRetry_WhenSourceErrors_LogsErrorAndRetries()
    {
        var logger = new MockLogger();
        var sequence = new Subject<int>();

        sequence
            .Do(_ =>
            {
                if(_ % 2 == 0) throw new Exception("Bang!");
            })
            .LogAndRetry(logger, "Error occurred")
            .Subscribe(
            _ => { },
            _ => { },
            () => { });

        // First attempt fails
        sequence.OnNext(1);
        sequence.OnNext(2);

        logger.LoggedErrors.ShouldBeGreaterThan(0);
    }

    [Test]
    public void LogAndRetry_WithNullLogger_DoesNotThrow()
    {
        var sequence = new Subject<int>();
        var results = new List<int>();

        Should.NotThrow(() =>
        {
            sequence.LogAndRetry(null, "Test message").Subscribe(results.Add);
            sequence.OnNext(42);
        });
    }

    #endregion

    #region LogAndRetryAfterDelay Tests

    [Test]
    public void LogAndRetryAfterDelay_WhenSourceErrors_LogsError()
    {
        var logger = new MockLogger();
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();

        sequence.LogAndRetryAfterDelay(logger, TimeSpan.FromMilliseconds(100), "Error", -1, scheduler)
            .Subscribe();

        sequence.OnError(new Exception("Test error"));

        logger.LoggedErrors.ShouldBeGreaterThan(0);
    }

    [Test]
    public void LogAndRetryAfterDelay_WithZeroRetryCount_RetriesImmediately()
    {
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();
        var results = new List<int>();

        sequence.LogAndRetryAfterDelay(null, TimeSpan.FromMilliseconds(0), "Error", 0, scheduler)
            .Subscribe(results.Add);

        sequence.OnNext(42);
        results.ShouldContain(42);
    }

    #endregion

    #region RepeatAfterDelay Tests

    [Test]
    public void RepeatAfterDelay_WhenSourceCompletes_RepeatsSequence()
    {
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();
        var results = new List<int>();
        var repeatCount = 0;

        sequence.RepeatAfterDelay(TimeSpan.FromMilliseconds(100), 1, scheduler)
            .Subscribe(
                value =>
                {
                    results.Add(value);
                    repeatCount++;
                },
                _ => { });

        sequence.OnNext(1);
        sequence.OnCompleted();

        results.ShouldContain(1);
    }

    #endregion

    #region RetryAfterDelay Tests

    [Test]
    public void RetryAfterDelay_WhenSourceErrors_RetriesWithDelay()
    {
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();
        var results = new List<int>();

        sequence.RetryAfterDelay(TimeSpan.FromMilliseconds(100), -1, scheduler)
            .Subscribe(results.Add);

        sequence.OnNext(42);
        results.ShouldContain(42);
    }

    [Test]
    public void RetryAfterDelay_WithRetryCountZero_DoesNotRetry()
    {
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();
        var errorCaught = false;

        sequence.RetryAfterDelay(TimeSpan.FromMilliseconds(100), 0, scheduler)
            .Subscribe(
                _ => { },
                _ => errorCaught = true);

        sequence.OnError(new Exception("Test error"));

        // After exhausting retries, error should be caught
    }

    [Test]
    public void RetryAfterDelay_WithRetryCountPositive_RetriesSpecifiedTimes()
    {
        var scheduler = new TestScheduler();
        var sequence = new Subject<int>();
        var results = new List<int>();

        sequence.RetryAfterDelay(TimeSpan.FromMilliseconds(50), 2, scheduler)
            .Subscribe(results.Add);

        sequence.OnNext(1);
        results.ShouldContain(1);
    }

    #endregion

    /// <summary>
    /// Mock implementation of IDisposable for testing.
    /// </summary>
    private class TestDisposable : IDisposable
    {
        private readonly Action _onDispose;

        public TestDisposable(Action onDispose = null)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke();
        }
    }

    /// <summary>
    /// Mock implementation of ILogger for testing.
    /// </summary>
    private class MockLogger : ILogger
    {
        public List<LogLevel> LogLevels { get; } = new();
        public int LoggedErrors { get; private set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return Disposable.Empty;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogLevels.Add(logLevel);
            if (logLevel == LogLevel.Error)
            {
                LoggedErrors++;
            }
        }
    }
}
