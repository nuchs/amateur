using Amateur;
using NUnit.Framework.Internal;
using Tests.Helpers;

namespace Tests;

internal class ActorTests
{
    [Test]
    public async Task StopMessageHaltsActor()
    {
        var unexpected = Guid.NewGuid();
        SpyPerformer spy = new();
        await using Actor sut = new(spy);

        var mb = PostOffice.Primary.GetMailbox(sut.Address);
        await mb.Deliver(Stop.Message);
        await mb.Deliver(unexpected);

        Assert.That(spy.Results, Does.Not.Contain(unexpected));
    }

    [Test]
    public async Task UsesPrimaryPostOfficeByDefault()
    {
        var expected = Guid.NewGuid().ToString();
        SpyPerformer spy = new();
        Address address = new();
        await using Actor sut = new(address, spy);

        await PostOffice.Primary.GetMailbox(address).Deliver(expected);
        await Task.Delay(1000);

        Assert.That(spy.Results, Contains.Item(expected));
    }

    [Test]
    public async Task UsesProvidedAddress()
    {
        Address address = new();
        await using Actor sut = new(address, new SpyPerformer());

        Assert.That(sut.Address, Is.EqualTo(address));
    }

    [Test]
    public async Task UsesProvidedPostOffice()
    {
        var expected = Guid.NewGuid().ToString();
        SpyPerformer spy = new();
        Address address = new();
        PostOffice postOffice = new();
        await using Actor sut = new(address, postOffice, spy);

        await postOffice.GetMailbox(address).Deliver(expected);
        await Task.Delay(1000);

        Assert.That(spy.Results, Contains.Item(expected));
    }
}
