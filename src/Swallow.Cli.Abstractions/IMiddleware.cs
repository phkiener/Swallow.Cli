namespace Swallow.Cli;

/// <summary>
/// A middleware that can be added to the invocation pipeline, before the actual <see cref="IEntrypoint"/> gets executed.
/// </summary>
public interface IMiddleware
{
    /// <summary>
    /// Run the middleware.
    /// </summary>
    /// <param name="context">The <see cref="ConsoleContext"/> for the invocation.</param>
    /// <param name="next">The next middleware to be executed.</param>
    public Task InvokeAsync(ConsoleContext context, InvocationDelegate next);
}
