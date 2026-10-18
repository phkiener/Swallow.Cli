using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Internal;

namespace Swallow.Cli;

partial class ConsoleApplication
{
    /// <summary>
    /// Use the given function as middleware.
    /// </summary>
    /// <param name="middleware">The middleware definition.</param>
    public void UseMiddleware(Func<ConsoleContext, InvocationDelegate, Task> middleware)
    {
        UseMiddleware(new InlineMiddleware(middleware));
    }

    /// <summary>
    /// Use the given <see cref="IMiddleware"/> as middleware.
    /// </summary>
    /// <param name="middleware">The middleware to use.</param>
    public void UseMiddleware(IMiddleware middleware)
    {
        middlewares.Add(prev => ctx => middleware.InvokeAsync(ctx, prev));
    }

    /// <summary>
    /// Add an instance of <typeparamref name="TMiddleware"/> as middleware.
    /// </summary>
    /// <typeparam name="TMiddleware">Type of middleware to use.</typeparam>
    /// <remarks>
    /// The type must be registered in the service collection of the <see cref="ConsoleApplicationBuilder"/>.
    /// </remarks>
    public void UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware
    {
        middlewares.Add(static prev => ctx => InvokeMiddleware(ctx, prev));

        static Task InvokeMiddleware(ConsoleContext context, InvocationDelegate next)
        {
            var middleware = context.Services.GetRequiredService<TMiddleware>();
            return middleware.InvokeAsync(context, next);
        }
    }
}
