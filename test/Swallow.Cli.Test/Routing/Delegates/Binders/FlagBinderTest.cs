using Swallow.Cli;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class FlagBinderTest
{
    private readonly FlagBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNotAFlag()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(bool), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsFalse();
        await Assert.That(boundValue).IsNull();
    }

    [Test]
    public void ThrowsException_WhenValueIsNotBoolean()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(int), [new FlagAttribute()], false, null);

        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    [Arguments(typeof(bool))]
    [Arguments(typeof(bool?))]
    public async Task BindsTrue_WhenFlagIsEnabled(Type parameterType)
    {
        var attribute = new FlagAttribute();

        var context = new TestConsoleContext
        {
            RouteValues = new RouteValues(
                flags: [attribute],
                options: new Dictionary<IOptionData, string>(),
                arguments: new Dictionary<IArgumentData, string>())
        };

        var bindingContext = new BindingContext(parameterType, [attribute], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsTypeOf<bool>().And.IsTrue();
    }

    [Test]
    [Arguments(typeof(bool))]
    [Arguments(typeof(bool?))]
    public async Task BindsFalse_WhenFlagIsNotEnabled(Type parameterType)
    {
        var context = new TestConsoleContext
        {
            RouteValues = new RouteValues(
                flags: [],
                options: new Dictionary<IOptionData, string>(),
                arguments: new Dictionary<IArgumentData, string>())
        };

        var bindingContext = new BindingContext(parameterType, [new FlagAttribute()], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsTypeOf<bool>().And.IsFalse();
    }
}
