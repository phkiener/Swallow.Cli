using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Internal;
using Swallow.Cli.Routing;

namespace Swallow.Cli;

/// <summary>
/// A console application capable of invoking <see cref="IEntrypoint"/>s.
/// </summary>
public sealed partial class ConsoleApplication : IDisposable, IAsyncDisposable
{
    private readonly ServiceProvider serviceProvider;

    private readonly List<IEntrypoint> entrypoints = [];
    private readonly List<Func<InvocationDelegate, InvocationDelegate>> middlewares = [];

    internal ConsoleApplication(IServiceCollection services)
    {
        serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Start a new invocation in this application.
    /// </summary>
    /// <param name="args">The arguments to use.</param>
    /// <returns>The exit code of the invocation.</returns>
    public async Task<int> RunAsync(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += CancelInvocation;

        try
        {
            var executionPipeline = BuildExecutionPipeline(middlewares, entrypoints);

            await using var scope = serviceProvider.CreateAsyncScope();
            var context = new DefaultConsoleContext(args, scope.ServiceProvider, cancellationTokenSource);

            await executionPipeline.Invoke(context);
            return context.Result.ExitCode ?? 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled exception: {e}");
            return 255;
        }
        finally
        {
            Console.CancelKeyPress -= CancelInvocation;
            cancellationTokenSource.Dispose();
        }

        void CancelInvocation(object? sender, ConsoleCancelEventArgs e)
        {
            // Apparently, a cancel can be cancelled so that it doesn't cancel the application. What.
            var shouldStop = e.Cancel is false;
            if (shouldStop)
            {
                try
                {
                    cancellationTokenSource.Cancel();
                }
                catch (ObjectDisposedException)
                {
                    // Eh, doesn't matter. Shouldn't happen, but if it does - so what.
                }
            }
        }
    }

    private static InvocationDelegate BuildExecutionPipeline(IReadOnlyList<Func<InvocationDelegate, InvocationDelegate>> middlewares, IReadOnlyList<IEntrypoint> entrypoints)
    {
        InvocationDelegate appDelegate = InvokeEntrypoint;

        for (var i = middlewares.Count - 1; i >= 0; i--)
        {
            appDelegate = middlewares[i](appDelegate);
        }

        var routing = new EntrypointRoutingMiddleware(entrypoints);
        return ctx => routing.InvokeAsync(ctx, appDelegate);

        static Task InvokeEntrypoint(ConsoleContext context)
        {
            return context.Entrypoint is null
                ? throw new InvalidOperationException("Execution pipeline did not set an entrypoint to be invoked.")
                : context.Entrypoint.InvokeAsync(context);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        serviceProvider.Dispose();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await serviceProvider.DisposeAsync();
    }

    /// <summary>
    /// Create a new <see cref="ConsoleApplicationBuilder"/>.
    /// </summary>
    /// <returns>The new builder.</returns>
    public static ConsoleApplicationBuilder CreateBuilder() => new();
}
