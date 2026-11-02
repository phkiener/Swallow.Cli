using Swallow.Cli.Routing.Segments;

namespace Swallow.Cli.Routing.Delegates;

/// <summary>
/// An attribute that defines an attribute for an <see cref="IEntrypoint"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ArgumentAttribute(bool isRequired) : Attribute, IArgumentData
{
    /// <inheritdoc />
    public bool IsRequired { get; } = isRequired;
}
