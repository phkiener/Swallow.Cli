namespace Swallow.Cli.Routing.Segments;

/// <summary>
/// A command, i.e. a prefix to choose a certain behavior.
/// </summary>
public interface ICommandData : IRoutePart
{
    /// <summary>
    /// Name of the command.
    /// </summary>
    public string Name { get; }
}
