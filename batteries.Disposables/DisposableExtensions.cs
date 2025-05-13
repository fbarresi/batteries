namespace batteries.Disposables;

public static class DisposableExtensions
{
    public static void AddDisposableTo(this IDisposable disposable, CompositeDisposable compositeDisposable)
    {
        compositeDisposable.Add(disposable);
    }
}
