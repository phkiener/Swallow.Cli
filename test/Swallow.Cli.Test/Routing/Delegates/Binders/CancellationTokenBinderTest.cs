using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class CancellationTokenBinderTest
{
    private readonly CancellationTokenBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNotBindingCancellationToken()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsFalse();
        await Assert.That(boundValue).IsNull();
    }

    [Test]
    public async Task BindsCancellationToken()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(CancellationToken), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo(context.Aborted);
    }
}
