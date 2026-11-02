using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Hosting;
using Swallow.Cli.Internal;

namespace Swallow.Cli;

/// <summary>
/// A builder for configuring a <see cref="ConsoleApplication"/>.
/// </summary>
public sealed class ConsoleApplicationBuilder
{
    /// <summary>
    /// A <see cref="IServiceCollection"/> to register any services to be resolved.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// The <see cref="IConsoleHostEnvironment"/> for the application.
    /// </summary>
    public IConsoleHostEnvironment Environment { get; }

    /// <summary>
    /// The <see cref="IConfiguration"/> and <see cref="IConfigurationBuilder"/> for the application.
    /// </summary>
    public IConfigurationManager Configuration { get; }

    internal ConsoleApplicationBuilder(string displayName, string? appIdentifier = null)
    {
        Services = new ServiceCollection();
        Environment = new DefaultConsoleHostEnvironment(displayName, appIdentifier ?? displayName);
        Configuration = new ConfigurationManager();
    }

    /// <summary>
    /// Construct a finished <see cref="ConsoleApplication"/> from this builder.
    /// </summary>
    /// <returns>The built application.</returns>
    public ConsoleApplication Build()
    {
        return new ConsoleApplication(Services, Environment, Configuration.Build());
    }
}
