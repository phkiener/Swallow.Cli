using Swallow.Cli.Routing.Delegates;

namespace Swallow.Cli.Routing;

public static class DelegateEntrypointExtensions
{
    public static void Map(this IEntrypointRouteBuilder routeBuilder, Delegate handler)
    {
        var dataSource = routeBuilder.GetOrAddDataSource<DelegateEntrypointDataSource>();
        dataSource.AddDelegate(handler);
    }

    public static void Map(this IEntrypointRouteBuilder routeBuilder, string command, Delegate handler)
    {
        var dataSource = routeBuilder.GetOrAddDataSource<DelegateEntrypointDataSource>();
        dataSource.AddDelegate(command.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries), handler);
    }
}
