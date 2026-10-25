namespace Swallow.Cli.Routing;

/// <summary>
/// An argument, i.e. a value passed to the command itself.
/// </summary>
public interface IArgumentData : IRoutePart
{
    /// <summary>
    /// Whether the flag is required.
    /// </summary>
    bool IsRequired { get; }
}
