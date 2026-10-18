using Microsoft.Extensions.DependencyInjection;

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

    internal ConsoleApplicationBuilder()
    {
        Services = new ServiceCollection();
    }

    /// <summary>
    /// Construct a finished <see cref="ConsoleApplication"/> from this builder.
    /// </summary>
    /// <returns>The built application.</returns>
    public ConsoleApplication Build()
    {
        return new ConsoleApplication(Services);
    }
}
