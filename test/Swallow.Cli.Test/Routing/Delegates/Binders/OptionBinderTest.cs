using Swallow.Cli;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
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
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(int), [new OptionAttribute()], false, null);

        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    public async Task BindsValue_WhenOptionHasValueAssigned()
    {
        var attribute = new OptionAttribute();

        var context = new TestConsoleContext
        {
            RouteValues = new RouteValues(
                flags: [],
                options: new Dictionary<IOptionData, string> { [attribute] = "value" },
                arguments: new Dictionary<IArgumentData, string>())
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
            RouteValues = new RouteValues(
                flags: [],
                options: new Dictionary<IOptionData, string>(),
                arguments: new Dictionary<IArgumentData, string>())
        };

        var bindingContext = new BindingContext(typeof(string), [new OptionAttribute()], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsNull();
    }
}
