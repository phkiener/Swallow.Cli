using Swallow.Console.Host.Invocation;

namespace Swallow.Console.Host.Routing;

public sealed class RoutingMiddleware : IMiddleware
{
    public Task InvokeAsync(InvocationContext invocationContext, InvocationHandler next)
    {
        // parse arguments
        // set endpoint

        return next.Invoke(invocationContext);
    }
}
