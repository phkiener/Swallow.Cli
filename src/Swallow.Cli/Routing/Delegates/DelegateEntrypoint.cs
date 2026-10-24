using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateEntrypoint : IEntrypoint
{
    internal DelegateEntrypoint(IRoutePart[] routeParts, InvocationDelegate @delegate)
    {
        RouteParts = routeParts;
        Delegate = @delegate;
    }

    public IRoutePart[] RouteParts { get; }
    public InvocationDelegate Delegate { get; }
}
