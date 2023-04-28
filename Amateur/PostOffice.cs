using System.Collections.Concurrent;

namespace Amateur;

public sealed class PostOffice
{
    private ConcurrentDictionary<Address, Mailbox> mailboxes = new();

    public static PostOffice Primary { get; } = new();

    public Mailbox GetMailbox(Address address)
    {
        if (mailboxes.TryGetValue(address, out var mb))
        {
            return mb;
        }

        throw new KeyNotFoundException($"No mailbox registered for address {address}");
    }

    internal Mailbox RegisterAddress(Address desiredAddress)
    {
        Mailbox mb = new();

        if (!mailboxes.TryAdd(desiredAddress, mb))
        {
            throw new ArgumentException($"A mailbox is already registered at '{desiredAddress}'", nameof(desiredAddress));
        }

        return mb;
    }

    internal bool TryDeregisterAddress(Address address)
        => mailboxes.TryRemove(address, out _);
}
