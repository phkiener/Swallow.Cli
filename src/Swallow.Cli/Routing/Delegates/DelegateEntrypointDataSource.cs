using Swallow.Cli.Routing.Segments;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateEntrypointDataSource : IEntrypointDataSource
{
    private readonly List<IEntrypoint> entrypoints = [];
    private readonly DelegateParser parser = new();
    private readonly DelegateInvoker invoker = new();

    public IReadOnlyCollection<IEntrypoint> GetEntrypoints() => entrypoints;

    public void AddDelegate(Delegate @delegate)
    {
        AddDelegate([], @delegate);
    }

    public void AddDelegate(string[] prefix, Delegate @delegate)
    {
        var command = prefix.Select(static p => new CommandData(p));
        var route = parser.ParseRoute(@delegate);

        var entrypoint = new DelegateEntrypoint([..command, ..route], c => invoker.InvokeDelegate(@delegate, c));
        entrypoints.Add(entrypoint);
    }

    private sealed record CommandData(string Name) : ICommandData;
}
