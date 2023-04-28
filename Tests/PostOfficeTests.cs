using Amateur;
using Tests.Helpers;

namespace Tests;

internal class PostOfficeTests
{
    [Test]
    public async Task ActorsAutoRegister()
    {
        Address address = new("blah");
        PostOffice sut = new();
        using StubPerformer performer = new();

        await using Actor act = new(address, sut, performer);

        Assert.That(sut.GetMailbox(address), Is.EqualTo(act.Mail));
    }

    [Test]
    public async Task CompletedActorsAutoDeregister()
    {
        Address address = new("blah");
        PostOffice sut = new();

        using (StubPerformer performer = new())
        await using (Actor act = new(address, sut, performer))
        {
            performer.Release();
        }

        Assert.Throws<KeyNotFoundException>(() => sut.GetMailbox(address));
    }

    [Test]
    public async Task DoubleRegistrationFails()
    {
        Address address = new("spunky");
        PostOffice sut = new();
        using StubPerformer performer = new();

        await using Actor act = new(address, sut, performer);

        Assert.Throws<ArgumentException>(() => new Actor(address, sut, performer));
    }

    [Test]
    public void ErrorOnUnregisteredAddress()
    {
        PostOffice sut = new();

        Assert.Throws<KeyNotFoundException>(() => sut.GetMailbox(new("no/such/address")));
    }
}
