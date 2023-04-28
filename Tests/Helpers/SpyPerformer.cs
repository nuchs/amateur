using Amateur;

namespace Tests.Helpers;

internal sealed class SpyPerformer : IPerformer
{
    public List<object> Results { get; } = new();

    public ValueTask<Performance> Perform(Actor actor, CancellationToken token, object message)
    {
        Results.Add(message);
        return ValueTask.FromResult(Performance.Ongoing);
    }
}
