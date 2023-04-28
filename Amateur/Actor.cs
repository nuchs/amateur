using System.Collections.Concurrent;

namespace Amateur;

public sealed class Actor : IAsyncDisposable, IDisposable
{
    private readonly ConcurrentBag<Mailbox> children = new();
    private readonly CancellationTokenSource cts = new();
    private Task eventLoopTak;
    private bool isDisposed;
    private IPerformer performer;
    private PostOffice postOffice;

    public Actor(Address address, PostOffice postOffice, IPerformer performer)
    {
        Mail = postOffice.RegisterAddress(address);
        this.postOffice = postOffice;
        this.performer = performer;
        Address = address;
        eventLoopTak = Task.Run(EventLoop);
    }

    public Actor(PostOffice postOffice, IPerformer performer)
        : this(new(), postOffice, performer)
    {
    }

    public Actor(Address address, IPerformer performer)
        : this(address, PostOffice.Primary, performer)
    {
    }

    public Actor(IPerformer performer)
        : this(new Address(), performer)
    {
    }

    public Address Address { get; } = new();

    public Mailbox Mail { get; }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        DisposeCore();
    }

    public async ValueTask DisposeAsync()
    {
        if (isDisposed)
        {
            return;
        }

        DisposeCore();
        await eventLoopTak.ConfigureAwait(false);
    }

    public Address SpawnChild(IPerformer childPerformer, Address? address = null)
    {
        if (isDisposed)
        {
            throw new ObjectDisposedException($"Actor {Address} has finished running");
        }

        Address childAddress = new($"{Address.Value}/{address?.Value ?? Guid.NewGuid().ToString()}");
        Actor child = new(childAddress, postOffice, childPerformer);

        children.Add(child.Mail);

        return child.Address;
    }

    private void DisposeCore()
    {
        isDisposed = true;
        postOffice.TryDeregisterAddress(Address);
        Mail.Closed = true;
        cts.Cancel();

        foreach (var child in children)
        {
            child.Deliver(Stop.Message);
        }
    }

    private async void EventLoop()
    {
        await foreach (var message in Mail.GetMail(cts.Token).ConfigureAwait(false))
        {
            if (cts.IsCancellationRequested ||
                message == Stop.Message ||
                await performer.Perform(this, cts.Token, message).ConfigureAwait(false) == Performance.Complete)
            {
                break;
            }
        }

        await DisposeAsync().ConfigureAwait(false);
    }
}
