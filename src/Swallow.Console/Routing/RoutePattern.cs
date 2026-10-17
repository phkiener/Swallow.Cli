namespace Swallow.Console.Routing;

public sealed class RoutePattern(IFlagData[] flags)
{
    public IFlagData[] Flags { get; } = flags;
}
