using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing;

public sealed class DelegateEntrypointTest
{
    [Test]
    public async Task BindsParameter_ConsoleContext()
    {
        ConsoleContext? capturedContext = null;

        var entrypoint = DelegateEntrypoint.Create(void (ConsoleContext context) => capturedContext = context);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedContext).IsSameReferenceAs(context);
    }

    [Test]
    public async Task BindsParameter_CancellationToken()
    {
        CancellationToken? capturedToken = null;

        var entrypoint = DelegateEntrypoint.Create(void (CancellationToken token) => capturedToken = token);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedToken).IsEqualTo(context.Aborted);
    }

    // TODO: Tests for flag-binding
    // I expect the routing to change once I finish all the other features,
    // so I'll skip these tests for now.

    [Test]
    public async Task BindsParameter_FromServiceAndRegistered_WithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromServiceAndRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromServiceAndNotRegistered_WithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task BindsParameter_FromServiceAndNotRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceUnkeyedAndRegistered_WithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceUnkeyedAndRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceUnkeyedAndNotRegistered_WithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceUnkeyedAndNotRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceNullKeyAndRegistered_WithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceNullKeyAndRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceNullKeyAndNotRegistered_WithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceNullKeyAndNotRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceWithKeyAndRegistered_WithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddKeyedSingleton<string>(1, "foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceWithKeyAndRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddKeyedSingleton<string>(1, "foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceWithKeyAndNotRegistered_WithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task BindsParameter_FromKeyedServiceWithKeyAndNotRegistered_WithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }
}
