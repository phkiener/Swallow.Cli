using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;
using Swallow.Cli.Routing.Delegates.Binders;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing.Delegates.Binders;

public sealed class FromServiceBinderTest
{
    private readonly FromServiceBinder binder = new();

    [Test]
    public async Task ReturnsFalse_WhenNeitherAttributeIsPresent()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsFalse();
        await Assert.That(boundValue).IsNull();
    }

    [Test]
    public async Task UnkeyedService_NoDefaultValue_Registered()
    {
        var context = new TestConsoleContext(static c => c.AddSingleton<string>("test"));
        var bindingContext = new BindingContext(typeof(string), [new FromServicesAttribute()], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("test");
    }

    [Test]
    public async Task UnkeyedService_DefaultValue_Registered()
    {
        var context = new TestConsoleContext(static c => c.AddSingleton<string>("test"));
        var bindingContext = new BindingContext(typeof(string), [new FromServicesAttribute()], true, "fallback");

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("test");
    }

    [Test]
    public async Task UnkeyedService_NoDefaultValue_NotRegistered()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [new FromServicesAttribute()], false, null);

        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    public async Task UnkeyedService_DefaultValue_NotRegistered()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [new FromServicesAttribute()], true, "fallback");

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("fallback");
    }

    [Test]
    public async Task KeyedService_NoDefaultValue_Registered()
    {
        var context = new TestConsoleContext(static c => c.AddKeyedSingleton<string>(1, "test"));
        var bindingContext = new BindingContext(typeof(string), [new FromKeyedServicesAttribute(1)], false, null);

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("test");
    }

    [Test]
    public async Task KeyedService_DefaultValue_Registered()
    {
        var context = new TestConsoleContext(static c => c.AddKeyedSingleton<string>(1, "test"));
        var bindingContext = new BindingContext(typeof(string), [new FromKeyedServicesAttribute(1)], true, "fallback");

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("test");
    }

    [Test]
    public async Task KeyedService_NoDefaultValue_NotRegistered()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [new FromKeyedServicesAttribute(1)], false, null);

        Assert.Throws<InvalidOperationException>(() => binder.TryBind(bindingContext, context, out _));
    }

    [Test]
    public async Task KeyedService_DefaultValue_NotRegistered()
    {
        var context = new TestConsoleContext();
        var bindingContext = new BindingContext(typeof(string), [new FromKeyedServicesAttribute(1)], true, "fallback");

        var result = binder.TryBind(bindingContext, context, out var boundValue);

        await Assert.That(result).IsTrue();
        await Assert.That(boundValue).IsEqualTo("fallback");
    }

}
