namespace Swallow.Cli.Abstractions;

/// <summary>
/// The result of an invocation.
/// </summary>
public sealed class InvocationResult
{
    /// <summary>
    /// The exit code to return.
    /// </summary>
    /// <remarks>
    /// Zero denotes success, while any non-zero code usually denotes an error.
    /// </remarks>
    public int? ExitCode { get; set; }
}
