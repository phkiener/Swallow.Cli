using Swallow.Console.Host.Routing;

namespace Swallow.Console.Host.Invocation;

public sealed class InvocationContext
{
    public required string[] Arguments { get; init; }
    public required CancellationToken CancellationToken { get; init; }

    public Entrypoint? Entrypoint { get; set; }
}
