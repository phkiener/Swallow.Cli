using Swallow.Cli.Hosting;

namespace Swallow.Cli.Routing;

/// <summary>
/// Extensions that configure the routing middleware, resolving a commandline invocation to an entrypoint.
/// </summary>
public static class EntrypointRoutingExtensions
{
    /// <summary>
    /// Enable the routing middleware.
    /// </summary>
    /// <param name="app">The host on which to add the middleware.</param>
    /// <remarks>
    /// The host will add the routing middleware automatically;
    /// adding it manually allows you to specify the exact point in the invocation pipeline.
    /// </remarks>
    public static void UseRouting(this IConsoleHost app)
    {
        app.UseMiddleware<EntrypointRoutingMiddleware>();
        app.Properties[EntrypointRoutingMiddleware.RoutingConfiguredKey] = new object();
    }
}
