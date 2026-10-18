namespace Swallow.Cli.Abstractions;

/// <summary>
/// An invocation of a commandline application.
/// </summary>
public sealed class Invocation(string[] arguments)
{
    /// <summary>
    /// The arguments passed via commandline.
    /// </summary>
    public string[] Arguments { get; } = arguments;

    /// <summary>
    /// The full commandline as single string.
    /// </summary>
    public string Commandline => string.Join(" ", Arguments);
}
