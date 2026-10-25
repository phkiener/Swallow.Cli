namespace Swallow.Cli;

/// <summary>
/// A delegate executing a command based on a <see cref="ConsoleContext"/>.
/// </summary>
public delegate Task InvocationDelegate(ConsoleContext context);
