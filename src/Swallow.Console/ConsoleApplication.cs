using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Console.Invocation;
using Swallow.Console.Routing;

namespace Swallow.Console;

public sealed class ConsoleApplication : IDisposable, IAsyncDisposable
{
    private readonly ServiceProvider serviceProvider;
    private readonly IConfigurationRoot configurationRoot;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    private readonly List<Entrypoint> entrypoints = [];
    private InvocationHandler appHandler = static ctx => ctx.Entrypoint?.Handler.Invoke(ctx) ?? Task.CompletedTask;

    public IServiceProvider ServiceProvider => serviceProvider;
    public IConfiguration Configuration => configurationRoot;

    internal ConsoleApplication(IServiceCollection services, IConfigurationBuilder configuration)
    {
        serviceProvider = services.BuildServiceProvider();
        configurationRoot = configuration.Build();
    }

    public void UseMiddleware(Func<InvocationContext, InvocationHandler, Task> handler)
    {
        var previousHandler = appHandler;
        appHandler = ctx => handler(ctx, previousHandler);
    }

    public void Use(IMiddleware middleware)
    {
        var previousHandler = appHandler;
        appHandler = ctx => middleware.InvokeAsync(ctx, previousHandler);
    }

    public void Use<TMiddleware>() where TMiddleware : IMiddleware
    {
        var previousHandler = appHandler;
        appHandler = ctx => ctx.ServiceProvider.GetRequiredService<TMiddleware>().InvokeAsync(ctx, previousHandler);
    }

    public void MapDefault(Delegate handler)
    {
        entrypoints.Add(Entrypoint.Parse(handler));
    }

    public async Task<int> RunAsync(string[] args)
    {
        System.Console.CancelKeyPress += CancelApplication;

        try
        {
            Use(new RoutingMiddleware(entrypoints));

            var context = new InvocationContext
            {
                Arguments = args,
                ServiceProvider = serviceProvider,
                CancellationToken = cancellationTokenSource.Token,
            };

            await appHandler.Invoke(context);

            return context.ExitCode ?? 0;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return 1;
        }
        finally
        {
            System.Console.CancelKeyPress -= CancelApplication;
        }
    }

    private void CancelApplication(object? sender, ConsoleCancelEventArgs e)
    {
        var shouldStop = e.Cancel is false;
        if (shouldStop)
        {
            cancellationTokenSource.Cancel();
        }
    }

    public void Dispose()
    {
        serviceProvider.Dispose();
        cancellationTokenSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await serviceProvider.DisposeAsync();
        cancellationTokenSource.Dispose();
    }

    public static ConsoleApplicationBuilder CreateBuilder(string displayName, string? applicationName = null)
        => new(displayName, applicationName ?? displayName);
}
