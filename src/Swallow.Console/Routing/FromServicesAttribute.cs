namespace Swallow.Console.Routing;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class FromServicesAttribute : Attribute
{
    public object? Key { get; set; }
}
