namespace Swallow.Cli.Hosting;

/// <summary>
/// The host of a console application
/// </summary>
public interface IConsoleHost
{
    /// <summary>
    /// The service provider for the application.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// The environment the application is hosted in.
    /// </summary>
    public IConsoleHostEnvironment Environment { get; }

    /// <summary>
    /// Add a middleware to the invocation pipeline.
    /// </summary>
    /// <param name="middleware">The middleware to use.</param>
    public IConsoleHost Use(Func<InvocationDelegate, InvocationDelegate> middleware);
}

/// <summary>
/// Quality of life extensions for the <see cref="IConsoleHost"/>.
/// </summary>
public static class ConsoleHostExtensions
{
    /// <summary>
    /// Add a middleware to the invocation pipeline.
    /// </summary>
    /// <param name="consoleHost">The <see cref="IConsoleHost"/> to which to add the middleware.</param>
    /// <typeparam name="TMiddleware">The type of middleware to add.</typeparam>
    /// <exception cref="InvalidOperationException">When <typeparamref name="TMiddleware"/> cannot be resolved from <see cref="IConsoleHost.Services"/>.</exception>
    public static IConsoleHost UseMiddleware<TMiddleware>(this IConsoleHost consoleHost) where TMiddleware : IMiddleware
    {
        return consoleHost.Use(static next => InvokeMiddleware(next));

        static InvocationDelegate InvokeMiddleware(InvocationDelegate next)
        {
            return ctx =>
            {
                if (ctx.Services.GetService(typeof(TMiddleware)) is not IMiddleware middleware)
                {
                    throw new InvalidOperationException($"Middleware {typeof(TMiddleware).Name} is not registered.");
                }

                return middleware.InvokeAsync(ctx, next);
            };
        }
    }

    /// <summary>
    /// Add a middleware to the invocation pipeline.
    /// </summary>
    /// <param name="consoleHost">The <see cref="IConsoleHost"/> to which to add the middleware.</param>
    /// <param name="middleware">The <see cref="IMiddleware"/> instance to use as middleware.</param>
    public static IConsoleHost UseMiddleware(this IConsoleHost consoleHost, IMiddleware middleware)
    {
        return consoleHost.Use(next => InvokeMiddleware(middleware, next));

        static InvocationDelegate InvokeMiddleware(IMiddleware middleware, InvocationDelegate next)
        {
            return ctx => middleware.InvokeAsync(ctx, next);
        }
    }

    /// <summary>
    /// Add a middleware to the invocation pipeline.
    /// </summary>
    /// <param name="consoleHost">The <see cref="IConsoleHost"/> to which to add the middleware.</param>
    /// <param name="middleware">The function to invoke as middleware.</param>
    public static IConsoleHost UseMiddleware(this IConsoleHost consoleHost, Func<ConsoleContext, InvocationDelegate, Task> middleware)
    {
        return consoleHost.Use(next => InvokeMiddleware(middleware, next));

        static InvocationDelegate InvokeMiddleware(Func<ConsoleContext, InvocationDelegate, Task> middleware, InvocationDelegate next)
        {
            return ctx => middleware.Invoke(ctx, next);
        }
    }
}
