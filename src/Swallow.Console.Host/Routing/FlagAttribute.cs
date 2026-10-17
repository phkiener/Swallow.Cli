namespace Swallow.Console.Host.Routing;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class FlagAttribute(bool optional = false) : Attribute
{
    public char ShortName { get; init; }
    public string? LongName { get; init; }
    public string[] Aliases { get; init; } = [];
}
