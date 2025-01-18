using System.Reactive.Disposables;

namespace batteries.Extensions;

public static class DisposableExtensions
{
    public static void AddDisposableTo(this IDisposable disposable, CompositeDisposable compositeDisposable)
    {
        compositeDisposable.Add(disposable);
    }
}
