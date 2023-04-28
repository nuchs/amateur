using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Amateur;

public class Mailbox
{
    private Channel<object> mail = Channel.CreateUnbounded<object>(new UnboundedChannelOptions()
    {
        SingleReader = true,
        SingleWriter = false
    });

    public bool Closed { get; internal set; }

    public ValueTask Deliver(object message)
    {
        if (Closed)
        {
            throw new InvalidOperationException("Cannot deliver message, mailbox is closed");
        }

        return mail.Writer.WriteAsync(message);
    }

    internal async IAsyncEnumerable<object> GetMail([EnumeratorCancellation] CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            object next;

            // Slightly weird construction because you can't have a yield in a try catch block
            try
            {
                next = await mail.Reader.ReadAsync(token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            yield return next;
        }
    }
}
