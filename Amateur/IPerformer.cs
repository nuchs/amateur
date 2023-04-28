namespace Amateur;

public interface IPerformer
{
    ValueTask<Performance> Perform(Actor actor, CancellationToken token, object message);
}
