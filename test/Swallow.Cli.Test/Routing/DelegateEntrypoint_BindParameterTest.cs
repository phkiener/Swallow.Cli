using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;
using Swallow.Cli.Routing;
using Swallow.Console.Arguments;

namespace Swallow.Console.Test.Routing;

public sealed class DelegateEntrypoint_BindParameterTest
{
    [Test]
    public async Task ConsoleContext()
    {
        ConsoleContext? capturedContext = null;

        var entrypoint = DelegateEntrypoint.Create(void (ConsoleContext context) => capturedContext = context);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedContext).IsSameReferenceAs(context);
    }

    [Test]
    public async Task CancellationToken()
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
    public async Task FromService_RegisteredWithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromService_RegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromService_NotRegisteredWithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task FromService_NotRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task FromKeyedService_UnkeyedAndRegisteredWithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_UnkeyedAndRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_UnkeyedAndNotRegisteredWithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task FromKeyedService_UnkeyedAndNotRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task FromKeyedService_NullKeyAndRegisteredWithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_NullKeyAndRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddSingleton<string>("foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_NullKeyAndNotRegisteredWithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task FromKeyedService_NullKeyAndNotRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(null)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }

    [Test]
    public async Task FromKeyedService_WithKeyAndRegisteredWithoutDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text) => capturedText = text);
        var context = new TestConsoleContext(services => services.AddKeyedSingleton<string>(1, "foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_WithKeyAndRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext(services => services.AddKeyedSingleton<string>(1, "foobar"));

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("foobar");
    }

    [Test]
    public async Task FromKeyedService_WithKeyAndNotRegisteredWithoutDefault()
    {
        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text) => {});
        var context = new TestConsoleContext();

        await Assert.ThrowsAsync(() => entrypoint.Delegate.Invoke(context))
            .WithMessageContaining("[FromKeyedServices]")
            .And.WithMessageContaining("not defined")
            .And.WithMessageContaining("no default");
    }

    [Test]
    public async Task FromKeyedService_WithKeyAndNotRegisteredWithDefault()
    {
        string? capturedText = null;

        var entrypoint = DelegateEntrypoint.Create(void ([FromKeyedServices(1)] string text = "fallback") => capturedText = text);
        var context = new TestConsoleContext();

        await entrypoint.Delegate.Invoke(context);
        await Assert.That(capturedText).IsEqualTo("fallback");
    }
}
