namespace Swallow.Console.Host.Routing;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class OptionAttribute(bool optional = false) : Attribute;
