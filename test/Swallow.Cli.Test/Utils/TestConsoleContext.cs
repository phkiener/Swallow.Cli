using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Abstractions;

namespace Swallow.Console.Arguments;

public class TestConsoleContext(string[] args, IServiceCollection services) : ConsoleContext
{
    public TestConsoleContext() : this([], new ServiceCollection()) { }
    public TestConsoleContext(string[] args) : this(args, new ServiceCollection()) { }
    public TestConsoleContext(IServiceCollection serviceCollection) : this([], serviceCollection) { }
    public TestConsoleContext(Action<IServiceCollection> configure) : this([], BuildServiceCollection(configure)) { }
    public TestConsoleContext(string[] args, Action<IServiceCollection> services) : this(args, BuildServiceCollection(services)) { }

    public override CancellationToken Aborted { get; } = CancellationToken.None;
    public override IServiceProvider Services { get; } = services.BuildServiceProvider();
    public override Invocation Invocation { get; } = new(args);
    public override InvocationResult Result { get; } = new();

    private static IServiceCollection BuildServiceCollection(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        configure(services);

        return services;
    }
}
