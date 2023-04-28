using Amateur;
using Tests.Helpers;

namespace Tests;

internal class MailBoxTests
{
    [Test]
    public void CantDeliverMessagesAfterDisposal()
    {
        SpyPerformer spy = new();
        Actor act = new(spy);
        var sut = act.Mail;

        act.Dispose();

        Assert.Throws<InvalidOperationException>(() => sut.Deliver(1));
    }

    [Test]
    public async Task MutliThreadedMessagesAreUnordered()
    {
        SpyPerformer spy = new();
        await using Actor act = new(spy);
        var sut = act.Mail;
        List<int> expected = new List<int> { 1, 2, 3 };

        Task Send(int msg) => Task.Run(() => sut.Deliver(msg));

        await Send(1);
        await Send(3);
        await Send(2);

        Assert.That(spy.Results, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task SingleThreadedMessagesAreOrdered()
    {
        SpyPerformer spy = new();
        await using Actor act = new(spy);
        var sut = act.Mail;
        List<int> expected = new List<int> { 1, 2, 3 };

        await sut.Deliver(1);
        await sut.Deliver(2);
        await sut.Deliver(3);

        Assert.That(spy.Results, Is.EqualTo(expected));
    }
}
