namespace Swallow.Cli;

/// <summary>
/// The context for the invocation of a console command.
/// </summary>
public abstract class ConsoleContext
{
    /// <summary>
    /// A <see cref="CancellationToken"/> that signals the abortion of the invocation.
    /// </summary>
    public abstract CancellationToken Aborted { get; }

    /// <summary>
    /// A <see cref="IServiceProvider"/> that provides the DI-registered services for the invocation.
    /// </summary>
    public abstract IServiceProvider Services { get; }

    /// <summary>
    /// The invocation itself.
    /// </summary>
    public abstract Invocation Invocation { get; }

    /// <summary>
    /// The result of the invocation.
    /// </summary>
    public abstract InvocationResult Result { get; }

    /// <summary>
    /// The routed <see cref="IEntrypoint"/> matching the <see cref="Invocation"/>, if any.
    /// </summary>
    public IEntrypoint? Entrypoint { get; set; }

    /// <summary>
    /// Contains parsed route information.
    /// </summary>
    public RouteValues? RouteValues { get; set; }
}
