using Swallow.Console.Host.Invocation;

namespace Swallow.Console.Host.Routing;

public sealed class Entrypoint(InvocationHandler handler)
{
    public InvocationHandler Handler { get; } = handler;
}
