using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing;

public sealed class EntrypointRoutingMiddlewareTest
{
    [Test]
    public async Task Flag_ShortName()
    {
        var context = await MatchRouteAsync(["-h"], [RoutePart.Flag(shortName: 'h')]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { ShortName: 'h' });
    }

    [Test]
    public async Task Flag_LongName()
    {
        var context = await MatchRouteAsync(["--help"], [RoutePart.Flag(longName: "help")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { LongName: "help" });
    }

    [Test]
    public async Task Command()
    {
        var context = await MatchRouteAsync(["foo"], [RoutePart.Command("foo")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).IsEmpty();
    }

    [Test]
    public async Task CommandAndFlag()
    {
        var context = await MatchRouteAsync(["foo", "-f"], [RoutePart.Command("foo"), RoutePart.Flag(shortName: 'f')]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { ShortName: 'f' });
    }

    [Test]
    public async Task Option_ShortName()
    {
        var context = await MatchRouteAsync(["-f", "bar"], [RoutePart.Option(shortName: 'f')]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.OptionValues).Contains(static kvp => kvp is { Key.ShortName: 'f', Value: "bar" });
    }

    [Test]
    public async Task Option_LongName()
    {
        var context = await MatchRouteAsync(["--foo", "bar"], [RoutePart.Option(longName: "foo")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.OptionValues).Contains(static kvp => kvp is { Key.LongName: "foo", Value: "bar" });
    }

    [Test]
    public async Task CommandAndOption()
    {
        var context = await MatchRouteAsync(["foo", "--bar", "quuz"], [RoutePart.Command("foo"), RoutePart.Option(longName: "bar")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.OptionValues).Contains(static kvp => kvp is { Key.LongName: "bar", Value: "quuz" });
    }

    [Test]
    public async Task FlagAndOption()
    {
        var context = await MatchRouteAsync(["-f", "--bar", "quuz"], [RoutePart.Flag(shortName: 'f'), RoutePart.Option(longName: "bar")]);

        await Assert.That(context.Entrypoint).IsNotNull();
        await Assert.That(context.RouteValues?.EnabledFlags).Contains(static f => f is { ShortName: 'f' });
        await Assert.That(context.RouteValues?.OptionValues).Contains(static kvp => kvp is { Key.LongName: "bar", Value: "quuz" });
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

        public static IOptionData Option(char shortName = '\0', string? longName = null, bool isRequired = false)
            => new OptionData(shortName, longName, isRequired);

        private sealed record CommandData(string Name) : ICommandData;
        private sealed record FlagData(char ShortName, string? LongName, bool IsRequired) : IFlagData;
        private sealed record OptionData(char ShortName, string? LongName, bool IsRequired) : IOptionData;
    }
}
