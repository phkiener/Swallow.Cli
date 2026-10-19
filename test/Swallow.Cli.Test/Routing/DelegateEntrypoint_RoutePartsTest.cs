using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing;

namespace Swallow.Console.Test.Routing;

public sealed class DelegateEntrypoint_RoutePartsTest
{
    [Test]
    public async Task GivenPrefix_IsAccepted()
    {
        var route = DelegateEntrypoint.Create([new CommandData("Name")], void () => { });

        await Assert.That(route.RouteParts).Count().IsEqualTo(1);
        await Assert.That(route.RouteParts[0] is ICommandData { Name: "Name" }).IsTrue();
    }

    [Test]
    public async Task Flag_IsAdded()
    {
        var route = DelegateEntrypoint.Create(void ([Flag(ShortName = 'c', LongName = "control", IsRequired = true)] bool enabled) => { });

        await Assert.That(route.RouteParts).Count().IsEqualTo(1);
        await Assert.That(route.RouteParts[0] is IFlagData { ShortName: 'c', LongName: "control", IsRequired: true }).IsTrue();
    }

    [Test]
    public async Task Option_IsAdded()
    {
        var route = DelegateEntrypoint.Create(void ([Option(ShortName = 'c', LongName = "control", IsRequired = true)] bool enabled) => { });

        await Assert.That(route.RouteParts).Count().IsEqualTo(1);
        await Assert.That(route.RouteParts[0] is IOptionData { ShortName: 'c', LongName: "control", IsRequired: true }).IsTrue();
    }

    [Test]
    public async Task Argument_IsAdded()
    {
        var route = DelegateEntrypoint.Create(void ([Argument(isRequired: true)] bool enabled) => { });

        await Assert.That(route.RouteParts).Count().IsEqualTo(1);
        await Assert.That(route.RouteParts[0] is IArgumentData { IsRequired: true }).IsTrue();
    }

    [Test]
    public async Task ComplexDelegate_RouteIsConstructed()
    {
        var route = DelegateEntrypoint.Create(
            [new CommandData("foo"), new CommandData("bar")],
            void ([Flag(ShortName = 'c')] bool a, [Option(LongName = "control")] bool b, [Argument(isRequired: false)] bool c) => { });

        await Assert.That(route.RouteParts).Count().IsEqualTo(5);
        await Assert.That(route.RouteParts[0] is ICommandData { Name: "foo" }).IsTrue();
        await Assert.That(route.RouteParts[1] is ICommandData { Name: "bar" }).IsTrue();
        await Assert.That(route.RouteParts[2] is IFlagData { ShortName: 'c' }).IsTrue();
        await Assert.That(route.RouteParts[3] is IOptionData { LongName: "control" }).IsTrue();
        await Assert.That(route.RouteParts[4] is IArgumentData { IsRequired: false }).IsTrue();
    }

    private sealed record CommandData(string Name) : ICommandData;
}
