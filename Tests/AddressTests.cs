using Amateur;
using Tests.Helpers;

namespace Tests;

internal class AddressTests
{
    [Test]
    public void RandomlyAssignedUniqueAddress()
    {
        Address sut1 = new();
        Address sut2 = new();

        Assert.That(sut1, Is.Not.EqualTo(sut2));
    }

    [TestCaseSource(typeof(RandomStrings))]
    public void SpecifyingAddress(string expected)
    {
        Address sut = new(expected);

        Assert.That(sut.Value, Is.EqualTo(expected));
    }

    [Test]
    public void ValuesAreNormalisedWhenCompared()
    {
        var address = "spank me";
        Address sut1 = new(address.ToUpper());
        Address sut2 = new(address.ToLower());

        Assert.That(sut1, Is.EqualTo(sut2));
    }
}
