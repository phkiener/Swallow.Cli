using Swallow.Cli.Abstractions;

namespace Swallow.Cli.Internal;

internal sealed class InlineMiddleware(Func<ConsoleContext, InvocationDelegate, Task> definition) : IMiddleware
{
    public Task InvokeAsync(ConsoleContext context, InvocationDelegate next)
    {
        return definition.Invoke(context, next);
    }
}
