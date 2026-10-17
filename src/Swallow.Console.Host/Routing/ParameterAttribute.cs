namespace Swallow.Console.Host.Routing;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class ParameterAttribute(bool optional = false) : Attribute;
