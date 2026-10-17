using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Swallow.Console.Host;

public sealed class ConsoleApplication : IDisposable, IAsyncDisposable
{
    private readonly ServiceProvider serviceProvider;
    private readonly IConfigurationRoot configurationRoot;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public IServiceProvider ServiceProvider => serviceProvider;
    public IConfiguration Configuration => configurationRoot;

    internal ConsoleApplication(IServiceCollection services, IConfigurationBuilder configuration)
    {
        serviceProvider = services.BuildServiceProvider();
        configurationRoot = configuration.Build();
    }

    public void Map(string command, Delegate handler)
    {
        return;
    }

    public void MapDefault(Delegate handler)
    {
        return;
    }

    public async Task<int> RunAsync(string[] args)
    {
        System.Console.CancelKeyPress += CancelApplication;

        try
        {
            await Task.CompletedTask;
            return 0;
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
