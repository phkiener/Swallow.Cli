using Swallow.Cli.Routing.Delegates;

namespace Swallow.Cli.Routing;

/// <summary>
/// Extensions to map a delegate-based entrypoint.
/// </summary>
public static class DelegateEntrypointExtensions
{
    /// <summary>
    /// Map the given <paramref name="handler"/> as <see cref="IEntrypoint"/>.
    /// </summary>
    /// <param name="routeBuilder">The <see cref="IEntrypointRouteBuilder"/> on which to add the entrypoint.</param>
    /// <param name="handler">The <see cref="Delegate"/> which will be used as entrypoint.</param>
    public static void Map(this IEntrypointRouteBuilder routeBuilder, Delegate handler)
    {
        var dataSource = routeBuilder.GetOrAddDataSource<DelegateEntrypointDataSource>();
        dataSource.AddDelegate(handler);
    }

    /// <summary>
    /// Map the given <paramref name="handler"/> as <see cref="IEntrypoint"/>.
    /// </summary>
    /// <param name="routeBuilder">The <see cref="IEntrypointRouteBuilder"/> on which to add the entrypoint.</param>
    /// <param name="command">A prefix for the <see cref="IEntrypoint"/>.</param>
    /// <param name="handler">The <see cref="Delegate"/> which will be used as entrypoint.</param>
    public static void Map(this IEntrypointRouteBuilder routeBuilder, string command, Delegate handler)
    {
        var dataSource = routeBuilder.GetOrAddDataSource<DelegateEntrypointDataSource>();
        dataSource.AddDelegate(command.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries), handler);
    }
}
