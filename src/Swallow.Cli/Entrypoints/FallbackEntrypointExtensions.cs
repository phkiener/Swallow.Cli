using Swallow.Cli.Hosting;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Segments;

namespace Swallow.Cli.Entrypoints;

/// <summary>
/// Extensions to map a handler for when no entrypoint matches the invocation.
/// </summary>
public static class FallbackEntrypointExtensions
{
    /// <summary>
    /// Map the given <paramref name="handler"/> to handle invocations where no other <see cref="IEntrypoint"/> matches.
    /// </summary>
    /// <param name="application">The application for which to add the fallback.</param>
    /// <param name="handler">The handler to invoke.</param>
    public static void MapFallback(this ConsoleApplication application, Func<ConsoleContext, Task> handler)
    {
        application.UseFallback(ctx => handler(ctx));
    }

    /// <summary>
    /// Map the given <paramref name="handler"/> to handle invocations where no other <see cref="IEntrypoint"/> matches.
    /// </summary>
    /// <param name="application">The application for which to add the fallback.</param>
    /// <param name="handler">The handler to invoke.</param>
    public static void MapFallback(this ConsoleApplication application, Action<ConsoleContext> handler)
    {
        application.UseFallback(ctx => { handler(ctx); return Task.CompletedTask; });
    }

    /// <summary>
    /// Map the given <paramref name="handler"/> to handle invocations where no other <see cref="IEntrypoint"/> matches.
    /// </summary>
    /// <param name="application">The application for which to add the fallback.</param>
    /// <param name="handler">The handler to invoke.</param>
    public static void MapFallback(this ConsoleApplication application, Func<Task> handler)
    {
        application.UseFallback(_ => handler());
    }

    /// <summary>
    /// Map the given <paramref name="handler"/> to handle invocations where no other <see cref="IEntrypoint"/> matches.
    /// </summary>
    /// <param name="application">The application for which to add the fallback.</param>
    /// <param name="handler">The handler to invoke.</param>
    public static void MapFallback(this ConsoleApplication application, Action handler)
    {
        application.UseFallback(_ => { handler(); return Task.CompletedTask; });
    }

    private static void UseFallback(this ConsoleApplication application, InvocationDelegate handler)
    {
        application.UseMiddleware(new UseFallbackMiddleware(new ImmediateEntrypoint(handler)));
    }

    private sealed class ImmediateEntrypoint(InvocationDelegate @delegate) : IEntrypoint
    {
        public IRoutePart[] RouteParts { get; } = [];
        public InvocationDelegate Delegate { get; } = @delegate;
    }

    private sealed class UseFallbackMiddleware(IEntrypoint fallbackEntrypoint) : IMiddleware
    {
        public Task InvokeAsync(ConsoleContext context, InvocationDelegate next)
        {
            context.Entrypoint ??= fallbackEntrypoint;
            return next(context);
        }
    }
}
