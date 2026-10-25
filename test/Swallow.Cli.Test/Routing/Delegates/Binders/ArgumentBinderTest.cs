using Swallow.Cli;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class ArgumentBinderTest
{
    private readonly ArgumentBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNotAnArgument()
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
        var bindingContext = new BindingContext(typeof(int), [new ArgumentAttribute(isRequired: true)], false, null);

        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    public async Task BindsValue_WhenArgumentHasValueAssigned()
    {
        var attribute = new ArgumentAttribute(isRequired: true);

        var context = new TestConsoleContext
        {
            RouteValues = new RouteValues(
                flags: [],
                options: new Dictionary<IOptionData, string>(),
                arguments: new Dictionary<IArgumentData, string> { [attribute] = "value" })
        };

        var bindingContext = new BindingContext(typeof(string), [attribute], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("value");
    }

    [Test]
    public async Task BindsNull_WhenArgumentHasNoValueAssigned()
    {
        var context = new TestConsoleContext
        {
            RouteValues = new RouteValues(
                flags: [],
                options: new Dictionary<IOptionData, string>(),
                arguments: new Dictionary<IArgumentData, string>())
        };

        var bindingContext = new BindingContext(typeof(string), [new ArgumentAttribute(isRequired: true)], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsNull();
    }
}
