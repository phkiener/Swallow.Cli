using Swallow.Cli.Hosting;

namespace Swallow.Cli.Internal;

internal sealed class DefaultConsoleHostEnvironment(string displayName, string appIdentifier) : IConsoleHostEnvironment
{
    public string DisplayName { get; } = displayName;
    public string AppIdentifier { get; } = appIdentifier;

    public string ConfigurationPath => Path.Combine(GetXdgDirectory("XDG_CONFIG_HOME", ".config"), AppIdentifier);
    public string CachePath => Path.Combine(GetXdgDirectory("XDG_CACHE_HOME", ".cache"), AppIdentifier);
    public string DataPath => Path.Combine(GetXdgDirectory("XDG_DATA_HOME", ".local/share"), AppIdentifier);
    public string StatePath => Path.Combine(GetXdgDirectory("XDG_STATE_HOME", ".local/state"), AppIdentifier);

    private static string GetXdgDirectory(string key, string fallbackLocation)
    {
        var envVar = Environment.GetEnvironmentVariable(key);
        if (envVar is not null)
        {
            return envVar;
        }

        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userHome, fallbackLocation);
    }

}
