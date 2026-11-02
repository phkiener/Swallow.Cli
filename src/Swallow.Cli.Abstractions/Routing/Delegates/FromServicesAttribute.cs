namespace Swallow.Cli.Routing.Delegates;

/// <summary>
/// An attribute that denotes a parameter to be injected via <see cref="ConsoleContext.Services"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class FromServicesAttribute : Attribute;
