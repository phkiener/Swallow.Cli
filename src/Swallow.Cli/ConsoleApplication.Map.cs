using Swallow.Cli.Abstractions;
using Swallow.Cli.Routing;

namespace Swallow.Cli;

partial class ConsoleApplication
{
    /// <summary>
    /// Map an entrypoint based on the given <see cref="Delegate"/>.
    /// </summary>
    /// <param name="handler">The delegate to map as <see cref="IEntrypoint"/>.</param>
    public void Map(Delegate handler)
    {
        entrypoints.Add(DelegateEntrypoint.Create(handler));
    }
}
