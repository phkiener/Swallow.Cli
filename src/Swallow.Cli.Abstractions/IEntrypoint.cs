using Swallow.Cli.Routing;

namespace Swallow.Cli;

/// <summary>
/// An entrypoint that may be invoked and routed to.
/// </summary>
public interface IEntrypoint
{
    /// <summary>
    /// The supported <see cref="IRoutePart"/>s.
    /// </summary>
    public IRoutePart[] RouteParts { get; }

    /// <summary>
    /// The <see cref="InvocationDelegate"/> used to invoke the entrypoint.
    /// </summary>
    public InvocationDelegate Delegate { get; }

    /// <summary>
    /// Directly invoke the entrypoint with the given <see cref="ConsoleContext"/>.
    /// </summary>
    /// <param name="context">The context for which to invoke the endpoint.</param>
    public Task InvokeAsync(ConsoleContext context) => Delegate.Invoke(context);
}
