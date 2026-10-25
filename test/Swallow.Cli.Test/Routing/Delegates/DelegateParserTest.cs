using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing.Delegates;

namespace Swallow.Console.Test.Routing.Delegates;

public sealed class DelegateParserTest
{
    private readonly DelegateParser parser = new();

    [Test]
    public async Task FlagAttribute_GetsAddedToRoute()
    {
        var routeParts = parser.ParseRoute(static void ([Flag(ShortName = 'd', LongName = "dry-run")] bool dryRun) => { });
        await Assert.That(routeParts).HasSingleItem(r => r is IFlagData { ShortName: 'd', LongName: "dry-run" });
    }

    [Test]
    public async Task OptionAttribute_GetsAddedToRoute()
    {
        var routeParts = parser.ParseRoute(static void ([Option(ShortName = 'c', LongName = "count")] int count) => { });
        await Assert.That(routeParts).HasSingleItem(r => r is IOptionData { ShortName: 'c', LongName: "count" });
    }

    [Test]
    public async Task ArgumentAttribute_GetsAddedToRoute()
    {
        var routeParts = parser.ParseRoute(static void ([Argument(isRequired: true)] int count) => { });
        await Assert.That(routeParts).HasSingleItem(r => r is IArgumentData { IsRequired: true });
    }

    [Test]
    public async Task ThrowsException_OnDuplicateShortName()
    {
        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Flag(ShortName = 'b')] bool a, [Flag(ShortName = 'b')] bool b) => { }));
        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Flag(LongName = "foo")] bool a, [Flag(LongName = "foo")] bool b) => { }));
        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Flag(ShortName = 'f', LongName = "foo")] bool a, [Flag(LongName = "foo")] bool b) => { }));

        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Option(ShortName = 'b')] bool a, [Option(ShortName = 'b')] bool b) => { }));
        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Option(LongName = "foo")] bool a, [Option(LongName = "foo")] bool b) => { }));
        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Option(ShortName = 'f', LongName = "foo")] bool a, [Option(LongName = "foo")] bool b) => { }));

        Assert.Throws<InvalidOperationException>(() => parser.ParseRoute(static void ([Flag(ShortName = 'b')] bool a, [Option(ShortName = 'b')] bool b) => { }));
    }

    [Test]
    public async Task UnattributedParameter_IsNotPartOfRoute()
    {
        var routeParts = parser.ParseRoute(static void (int _, string _) => { });
        await Assert.That(routeParts).IsEmpty();
    }
}
