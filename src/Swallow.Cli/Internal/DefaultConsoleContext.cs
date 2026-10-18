using Swallow.Cli.Abstractions;

namespace Swallow.Cli.Internal;

internal sealed class DefaultConsoleContext(
    string[] arguments,
    IServiceProvider services,
    CancellationTokenSource cancellationTokenSource)
    : ConsoleContext
{
    public override CancellationToken Aborted { get; } = cancellationTokenSource.Token;
    public override IServiceProvider Services { get; } = services;
    public override Invocation Invocation { get; } = new(arguments);
    public override InvocationResult Result { get; } = new();
}
