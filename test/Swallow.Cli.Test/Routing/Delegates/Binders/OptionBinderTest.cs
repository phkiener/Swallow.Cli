using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Cli.Routing.Segments;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class OptionBinderTest
{
    private readonly OptionBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNotAnOption()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsFalse();
        await Assert.That(boundValue).IsNull();
    }

    [Test]
    public void ThrowsException_WhenValueIsNotString()
    {
        var attribute = new OptionAttribute();
        var context = new TestConsoleContext
        {
            RouteValues = new Dictionary<IRoutePart, string> { [attribute] = "value" }
        };

        var bindingContext = new BindingContext(typeof(TestConsoleContext), [attribute], false, null);
        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    public async Task BindsValue_WhenOptionHasValueAssigned()
    {
        var attribute = new OptionAttribute();

        var context = new TestConsoleContext
        {
            RouteValues = new Dictionary<IRoutePart, string> { [attribute] = "value" }
        };

        var bindingContext = new BindingContext(typeof(string), [attribute], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("value");
    }

    [Test]
    public async Task BindsNull_WhenOptionHasNoValueAssigned()
    {
        var context = new TestConsoleContext
        {
            RouteValues = new Dictionary<IRoutePart, string>()
        };

        var bindingContext = new BindingContext(typeof(string), [new OptionAttribute()], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsNull();
    }
}
