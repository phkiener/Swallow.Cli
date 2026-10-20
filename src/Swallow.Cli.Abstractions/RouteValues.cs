using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Abstractions;

/// <summary>
/// Parsed route information.
/// </summary>
public sealed class RouteValues(IEnumerable<IFlagData> flags, IDictionary<IOptionData, string> options)
{
    /// <summary>
    /// All flags that have been enabled.
    /// </summary>
    public IReadOnlySet<IFlagData> EnabledFlags { get; } = flags.ToHashSet();

    /// <summary>
    /// All option values that have been passed.
    /// </summary>
    public IReadOnlyDictionary<IOptionData, string> OptionValues { get; } = options.AsReadOnly();

    /// <summary>
    /// A shared instance of empty route values.
    /// </summary>
    public static RouteValues Empty { get; } = new([], new Dictionary<IOptionData, string>());
}
