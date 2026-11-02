using Swallow.Cli.Internal;

namespace Swallow.Console.Test.Internal;

[NotInParallel(nameof(Environment))]
public sealed class DefaultConsoleHostEnvironmentTest
{
    [Test]
    public async Task UsesEnvironmentVariables_ForPaths()
    {
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", "/configs");
        Environment.SetEnvironmentVariable("XDG_CACHE_HOME", "/cache");
        Environment.SetEnvironmentVariable("XDG_DATA_HOME", "/data");
        Environment.SetEnvironmentVariable("XDG_STATE_HOME", "/state");

        var environment = new DefaultConsoleHostEnvironment(displayName: "name", appIdentifier: "app.name");

        await Assert.That(environment.ConfigurationPath).IsEqualTo("/configs/app.name");
        await Assert.That(environment.CachePath).IsEqualTo("/cache/app.name");
        await Assert.That(environment.DataPath).IsEqualTo("/data/app.name");
        await Assert.That(environment.StatePath).IsEqualTo("/state/app.name");
    }

    [Test]
    public async Task UsesFallbackPaths_WhenEnvironmentVariablesNotDefined()
    {
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", null);
        Environment.SetEnvironmentVariable("XDG_CACHE_HOME", null);
        Environment.SetEnvironmentVariable("XDG_DATA_HOME", null);
        Environment.SetEnvironmentVariable("XDG_STATE_HOME", null);

        var environment = new DefaultConsoleHostEnvironment(displayName: "name", appIdentifier: "app.name");

        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        await Assert.That(environment.ConfigurationPath).IsEqualTo($"{userHome}/.config/app.name");
        await Assert.That(environment.CachePath).IsEqualTo($"{userHome}/.cache/app.name");
        await Assert.That(environment.DataPath).IsEqualTo($"{userHome}/.local/share/app.name");
        await Assert.That(environment.StatePath).IsEqualTo($"{userHome}/.local/state/app.name");
    }
}
