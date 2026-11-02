namespace Swallow.Cli.Hosting;

/// <summary>
/// The environment a console application is running in.
/// </summary>
public interface IConsoleHostEnvironment
{
    /// <summary>
    /// The user-visible display name of the application.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// The unique app identifier for the application.
    /// </summary>
    public string AppIdentifier { get; }

    /// <summary>
    /// Path to the configuration files for the application.
    /// </summary>
    /// <remarks>This corresponds to <c>XDG_CONFIG_HOME</c>; the past might not exist yet.</remarks>
    public string ConfigurationPath { get; }

    /// <summary>
    /// Path to the cached data for the application.
    /// </summary>
    /// <remarks>This corresponds to <c>XDG_CACHE_HOME</c>; the past might not exist yet.</remarks>
    public string CachePath { get; }

    /// <summary>
    /// Path to the data files for the application.
    /// </summary>
    /// <remarks>This corresponds to <c>XDG_DATA_HOME</c>; the past might not exist yet.</remarks>
    public string DataPath { get; }

    /// <summary>
    /// Path to the state files for the application.
    /// </summary>
    /// <remarks>This corresponds to <c>XDG_STATE_HOME</c>; the past might not exist yet.</remarks>
    public string StatePath { get; }
}
