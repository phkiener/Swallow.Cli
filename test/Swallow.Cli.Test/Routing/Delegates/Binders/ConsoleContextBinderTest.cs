using Swallow.Cli;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class ConsoleContextBinderTest
{
    private readonly ConsoleContextBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNotBindingConsoleContext()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsFalse();
        await Assert.That(boundValue).IsNull();
    }

    [Test]
    public async Task BindsConsoleContext()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(ConsoleContext), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsSameReferenceAs(context);
    }
}
