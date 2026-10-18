using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Abstractions;

/// <summary>
/// Parsed route information.
/// </summary>
public sealed class RouteValues(IEnumerable<IFlagData> flags)
{
    /// <summary>
    /// All flags that have been enabled.
    /// </summary>
    public IReadOnlySet<IFlagData> EnabledFlags { get; } = flags.ToHashSet();
}
