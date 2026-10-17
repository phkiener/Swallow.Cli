namespace Swallow.Console.Host.Routing;

public sealed class RoutePattern(IFlagData[] flags)
{
    public IFlagData[] Flags { get; } = flags;
}
