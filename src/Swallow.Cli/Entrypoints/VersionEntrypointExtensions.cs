using System.Reflection;
using Swallow.Cli.Hosting;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;

namespace Swallow.Cli.Entrypoints;

/// <summary>
/// Helper methods to add a version endpoint to the application.
/// </summary>
public static class VersionEntrypointExtensions
{
    /// <summary>
    /// Add a minimal entrypoint that prints the version number.
    /// </summary>
    /// <param name="app">The route builder to which to add this entrypoint.</param>
    /// <param name="displayAppName">Whether the app name should be displayed as part of the output.</param>
    public static void MapAssemblyVersion(this IEntrypointRouteBuilder app, bool displayAppName = false)
    {
        app.Map(([Flag(ShortName = 'v', LongName = "version", IsRequired = true)] bool unused, [FromServices] IConsoleHostEnvironment environment) =>
        {
            var version = GetAssemblyInformationalVersion() ?? GetAssemblyVersion() ?? "unknown";
            Console.WriteLine(displayAppName ? $"{environment.DisplayName} version {version}" : version);
        });
    }

    /// <summary>
    /// Add a minimal entrypoint that prints the version number.
    /// </summary>
    /// <param name="app">The route builder to which to add this entrypoint.</param>
    /// <param name="versionText">The version information to display.</param>
    /// <param name="displayAppName">Whether the app name should be displayed as part of the output.</param>
    public static void MapVersion(this IEntrypointRouteBuilder app, string versionText, bool displayAppName = false)
    {
        app.Map(([Flag(ShortName = 'v', LongName = "version", IsRequired = true)] bool unused, [FromServices] IConsoleHostEnvironment environment) =>
        {
            Console.WriteLine(displayAppName ? $"{environment.DisplayName} version {versionText}" : versionText);
        });
    }

    private static string? GetAssemblyInformationalVersion()
    {
        return Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }

    private static string? GetAssemblyVersion()
    {
        return Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
    }
}
