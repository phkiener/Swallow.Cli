using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Swallow.Cli.Routing;

/// <summary>
/// Contains parsed route information, i.e. the value associated with to every <see cref="IRoutePart"/>.
/// </summary>
public sealed class RouteValueDictionary : IReadOnlyDictionary<IRoutePart, string>
{
    private readonly IReadOnlyDictionary<IRoutePart, string> values;

    /// <summary>
    /// Create a new <see cref="RouteValueDictionary"/> based on the given route values.
    /// </summary>
    /// <param name="source">A collection of route parts and their associated value.</param>
    public RouteValueDictionary(params IEnumerable<KeyValuePair<IRoutePart, string>> source)
    {
        values = source.ToDictionary(static x => x.Key, static x => x.Value);
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<IRoutePart, string>> GetEnumerator() => values.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

    /// <inheritdoc />
    public int Count => values.Count;

    /// <inheritdoc />
    public bool ContainsKey(IRoutePart key) => values.ContainsKey(key);

    /// <inheritdoc />
    public bool TryGetValue(IRoutePart key, [MaybeNullWhen(false)] out string value) => values.TryGetValue(key, out value);

    /// <inheritdoc />
    public string this[IRoutePart key] => values[key];

    /// <inheritdoc />
    public IEnumerable<IRoutePart> Keys => values.Keys;

    /// <inheritdoc />
    public IEnumerable<string> Values => values.Values;

    /// <summary>
    /// An empty route value dictionary.
    /// </summary>
    public static RouteValueDictionary Empty { get; } = new();
}
