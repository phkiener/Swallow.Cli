namespace Swallow.Cli.Routing;

/// <summary>
/// An attribute that defines an option for an <see cref="IEntrypoint"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class OptionAttribute : Attribute, IOptionData
{
    /// <inheritdoc />
    public bool IsRequired { get; init; }

    /// <inheritdoc />
    public char ShortName { get; init; }

    /// <inheritdoc />
    public string? LongName { get; init; }

    /// <inheritdoc />
    public override string ToString()
    {
        var shortName = ShortName is '\0' ? null : $"-{ShortName} $";
        var longName = LongName is null ? null : $"--{LongName} $";
        var names = new[] { shortName, longName };

        return IsRequired ? $"<{string.Join("|", names)}>" : $"[{string.Join("|", names)}]";
    }
}
