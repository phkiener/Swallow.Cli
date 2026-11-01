namespace Swallow.Cli.Routing;

public static class FallbackEntrypointExtensions
{
    public static void MapFallback(this ConsoleApplication application, Func<ConsoleContext, Task> handler)
    {
        application.UseFallback(ctx => handler(ctx));
    }

    public static void MapFallback(this ConsoleApplication application, Action<ConsoleContext> handler)
    {
        application.UseFallback(ctx => { handler(ctx); return Task.CompletedTask; });
    }

    public static void MapFallback(this ConsoleApplication application, Func<Task> handler)
    {
        application.UseFallback(_ => handler());
    }

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
