namespace Swallow.Cli.Abstractions.Routing;

/// <summary>
/// An option, i.e. a <see cref="IFlagData"/> with a value.
/// </summary>
public interface IOptionData : IRoutePart
{
    /// <summary>
    /// Whether the flag is required.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// A single character that enables this flag.
    /// </summary>
    /// <remarks>
    /// UNIX-style activation is with a single dash (<c>-c</c>),
    /// Windows-Style activation is with a single slash (<c>/c</c>).
    /// </remarks>
    char ShortName { get; }

    /// <summary>
    /// A long name that enables this flag.
    /// </summary>
    /// <remarks>
    /// UNIX-style activation is with a double dash (usually kebab-case) (<c>--long-name</c>),
    /// Windows-Style activation is with a single slash (usually PascalCase) (<c>/LongName</c>).
    /// <br/>
    /// Do not include a double-dash or a slash, the prefix is handled automatically.
    /// </remarks>
    string? LongName { get; }
}
