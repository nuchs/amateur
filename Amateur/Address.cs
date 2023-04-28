namespace Amateur;

public sealed record Address
{
    public Address(string value) => Value = value;

    public Address()
        : this(Guid.NewGuid().ToString())
    {
    }

    public string Value { get; set; }

    public bool Equals(Address? obj)
        => obj is Address other && other.Value.ToLower() == Value.ToLower();

    public override int GetHashCode()
        => Value.ToLower().GetHashCode();
}
