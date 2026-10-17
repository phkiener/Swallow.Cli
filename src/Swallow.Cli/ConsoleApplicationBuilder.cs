using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Swallow.Cli;

public sealed class ConsoleApplicationBuilder
{
    public IServiceCollection Services { get; }
    public IConfigurationBuilder Configuration { get; }

    internal ConsoleApplicationBuilder(string displayName, string applicationName)
    {
        Services = new ServiceCollection();
        Configuration = new ConfigurationBuilder();
    }

    public ConsoleApplication Build()
    {
        return new ConsoleApplication(Services, Configuration);
    }
}
