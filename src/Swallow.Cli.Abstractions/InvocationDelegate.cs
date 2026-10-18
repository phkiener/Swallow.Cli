namespace Swallow.Cli.Abstractions;

/// <summary>
/// A delegate executing a command based on a <see cref="ConsoleContext"/>.
/// </summary>
public delegate Task InvocationDelegate(ConsoleContext context);
