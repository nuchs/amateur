using Amateur;

namespace Tests.Helpers;

internal class StubPerformer : IPerformer, IDisposable
{
    private ManualResetEvent flag = new(false);

    public void Dispose() => Release();

    public ValueTask<Performance> Perform(Actor actor, CancellationToken token, object message)
    {
        flag.WaitOne();
        return ValueTask.FromResult(Performance.Complete);
    }

    public void Release() => flag.Set();
}
