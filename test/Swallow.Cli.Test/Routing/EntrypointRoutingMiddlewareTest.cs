using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing;

public sealed class EntrypointRoutingMiddlewareTest
{
    [Test]
    public async Task Help_ShortName()
    {
        var context = await MatchRouteAsync(["-h"], [RoutePart.Flag(shortName: 'h')]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { ShortName: 'h' });
    }

    [Test]
    public async Task Help_LongName()
    {
        var context = await MatchRouteAsync(["--help"], [RoutePart.Flag(longName: "help")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { LongName: "help" });
    }

    [Test]
    public async Task Prefix()
    {
        var context = await MatchRouteAsync(["foo"], [RoutePart.Command("foo")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).IsEmpty();
    }

    [Test]
    public async Task PrefixAndFlag()
    {
        var context = await MatchRouteAsync(["foo", "-f"], [RoutePart.Command("foo"), RoutePart.Flag(shortName: 'f')]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { ShortName: 'f' });
    }

    private static async Task<TestConsoleContext> MatchRouteAsync(string[] args, IRoutePart[] routeParts)
    {
        var context = new TestConsoleContext(args);
        var entrypoint = new TestEntrypoint(routeParts);
        var middleware = new EntrypointRoutingMiddleware([entrypoint]);

        await middleware.InvokeAsync(context, _ => Task.CompletedTask);

        return context;
    }

    private sealed class TestEntrypoint(params IRoutePart[] routeParts) : IEntrypoint
    {
        public IRoutePart[] RouteParts { get; } = routeParts;
        public InvocationDelegate Delegate { get; } = _ => Task.CompletedTask;
    }

    private static class RoutePart
    {
        public static ICommandData Command(string name) => new CommandData(name);

        public static IFlagData Flag(char shortName = '\0', string? longName = null, bool isRequired = false)
            => new FlagData(shortName, longName, isRequired);

        private sealed record CommandData(string Name) : ICommandData;
        private sealed record FlagData(char ShortName, string? LongName, bool IsRequired) : IFlagData;
    }
}
