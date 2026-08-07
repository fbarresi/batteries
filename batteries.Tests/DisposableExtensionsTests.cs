using batteries.Extensions;
using Shouldly;
using System.Reactive.Disposables;

namespace batteries.Tests;

public class DisposableExtensionsTests
{
    #region AddDisposableTo Tests

    [Test]
    public void AddDisposableTo_WithValidDisposable_AddsToComposite()
    {
        var compositeDisposable = new CompositeDisposable();
        var mockDisposable = new MockDisposable();

        mockDisposable.AddDisposableTo(compositeDisposable);

        compositeDisposable.Count.ShouldBe(1);
    }

    [Test]
    public void AddDisposableTo_WithMultipleDisposables_AddsAllToComposite()
    {
        var compositeDisposable = new CompositeDisposable();
        var disposable1 = new MockDisposable();
        var disposable2 = new MockDisposable();
        var disposable3 = new MockDisposable();

        disposable1.AddDisposableTo(compositeDisposable);
        disposable2.AddDisposableTo(compositeDisposable);
        disposable3.AddDisposableTo(compositeDisposable);

        compositeDisposable.Count.ShouldBe(3);
    }

    [Test]
    public void AddDisposableTo_WhenCompositeIsDisposed_DisposesAddedDisposable()
    {
        var compositeDisposable = new CompositeDisposable();
        var mockDisposable = new MockDisposable();

        mockDisposable.AddDisposableTo(compositeDisposable);
        compositeDisposable.Dispose();

        mockDisposable.IsDisposed.ShouldBeTrue();
    }

    [Test]
    public void AddDisposableTo_WithMultipleDisposables_DisposesAllWhenCompositeDisposed()
    {
        var compositeDisposable = new CompositeDisposable();
        var disposable1 = new MockDisposable();
        var disposable2 = new MockDisposable();
        var disposable3 = new MockDisposable();

        disposable1.AddDisposableTo(compositeDisposable);
        disposable2.AddDisposableTo(compositeDisposable);
        disposable3.AddDisposableTo(compositeDisposable);

        compositeDisposable.Dispose();

        disposable1.IsDisposed.ShouldBeTrue();
        disposable2.IsDisposed.ShouldBeTrue();
        disposable3.IsDisposed.ShouldBeTrue();
    }

    [Test]
    public void AddDisposableTo_WithDisposable_ImmediatelyAdded()
    {
        var compositeDisposable = new CompositeDisposable();
        var mockDisposable = new MockDisposable();

        var countBefore = compositeDisposable.Count;
        mockDisposable.AddDisposableTo(compositeDisposable);
        var countAfter = compositeDisposable.Count;

        countBefore.ShouldBe(0);
        countAfter.ShouldBe(1);
    }

    [Test]
    public void AddDisposableTo_AddingSameDisposableTwice_AddsMultipleTimes()
    {
        var compositeDisposable = new CompositeDisposable();
        var mockDisposable = new MockDisposable();

        mockDisposable.AddDisposableTo(compositeDisposable);
        mockDisposable.AddDisposableTo(compositeDisposable);

        compositeDisposable.Count.ShouldBe(2);
    }

    [Test]
    public void AddDisposableTo_WithActionDisposable_AddsSuccessfully()
    {
        var compositeDisposable = new CompositeDisposable();
        var disposed = false;
        var actionDisposable = Disposable.Create(() => disposed = true);

        actionDisposable.AddDisposableTo(compositeDisposable);
        compositeDisposable.Count.ShouldBe(1);

        compositeDisposable.Dispose();
        disposed.ShouldBeTrue();
    }

    [Test]
    public void AddDisposableTo_WithEmptyDisposable_AddsSuccessfully()
    {
        var compositeDisposable = new CompositeDisposable();
        var emptyDisposable = Disposable.Empty;

        emptyDisposable.AddDisposableTo(compositeDisposable);

        compositeDisposable.Count.ShouldBe(1);
    }

    [Test]
    public void AddDisposableTo_AfterCompositeDisposed_DoesNotPreventOperations()
    {
        var compositeDisposable = new CompositeDisposable();
        compositeDisposable.Dispose();

        var mockDisposable = new MockDisposable();
        Should.NotThrow(() => mockDisposable.AddDisposableTo(compositeDisposable));
    }

    #endregion

    /// <summary>
    /// Mock implementation of IDisposable for testing purposes.
    /// </summary>
    private class MockDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
