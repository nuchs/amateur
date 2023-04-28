using System.Collections;

namespace Tests.Helpers;

internal sealed class RandomStrings : IEnumerable<string>
{
    private static readonly char[] chars =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '`', '¬', '!', '"', '£', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '+',
        '[', ']', '{', '}', ';', ':', '\'', '@', '#', '~', '\\', '|', ',', '.', '<', '>', '/', '?'
    };

    private static readonly Random rand = new();

    public IEnumerator<string> GetEnumerator()
        => Enumerable.Range(1, 5).Select(_ => RandomString()).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal static char RandomChar()
        => chars[rand.Next(chars.Length)];

    internal static string RandomString()
        => string.Join("", Enumerable.Range(1, rand.Next(255)).Select(_ => RandomChar()));
}
