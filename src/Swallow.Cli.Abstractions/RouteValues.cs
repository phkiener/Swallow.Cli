using Swallow.Cli.Routing;

namespace Swallow.Cli;

/// <summary>
/// Parsed route information.
/// </summary>
public sealed class RouteValues(IEnumerable<IFlagData> flags, IDictionary<IOptionData, string> options, IDictionary<IArgumentData, string> arguments)
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
    /// All arguments that have been passed.
    /// </summary>
    public IReadOnlyDictionary<IArgumentData, string> Arguments { get; } = arguments.AsReadOnly();

    /// <summary>
    /// A shared instance of empty route values.
    /// </summary>
    public static RouteValues Empty { get; } = new([], new Dictionary<IOptionData, string>(), new Dictionary<IArgumentData, string>());
}
