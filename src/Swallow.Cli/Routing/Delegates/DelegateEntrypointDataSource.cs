using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateEntrypointDataSource : IEntrypointDataSource
{
    private readonly List<Delegate> delegates = [];
    private readonly DelegateParser parser = new();
    private readonly DelegateInvoker invoker = new();

    private IReadOnlyCollection<IEntrypoint>? resolvedEntrypoints;

    public IReadOnlyCollection<IEntrypoint> GetEntrypoints()
    {
        resolvedEntrypoints ??= delegates.Select(BuildEntrypoint).ToList();
        return resolvedEntrypoints;
    }

    public void AddDelegate(Delegate @delegate)
    {
        delegates.Add(@delegate);
    }

    private IEntrypoint BuildEntrypoint(Delegate @delegate)
    {
        var route = parser.ParseRoute(@delegate);

        return new DelegateEntrypoint(route, c => invoker.InvokeDelegate(@delegate, c));
    }
}
