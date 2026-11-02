using Microsoft.Extensions.Configuration;
using Swallow.Cli;
using Swallow.Cli.Entrypoints;
using Swallow.Cli.Hosting;
using Swallow.Cli.Routing;
using Swallow.Cli.Routing.Delegates;

Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", Environment.CurrentDirectory + "/.run/XDG_CONFIG_HOME");
Environment.SetEnvironmentVariable("XDG_CACHE_HOME", Environment.CurrentDirectory + "/.run/XDG_CACHE_HOME");
Environment.SetEnvironmentVariable("XDG_DATA_HOME", Environment.CurrentDirectory + "/.run/XDG_DATA_HOME");
Environment.SetEnvironmentVariable("XDG_STATE_HOME", Environment.CurrentDirectory + "/.run/XDG_STATE_HOME");

var builder = ConsoleApplication.CreateBuilder("demo");
builder.Configuration.AddEnvironmentVariables("DEMO_");
builder.Configuration.AddJsonFile($"{builder.Environment.ConfigurationPath}/cfg.json", optional: true);

var app = builder.Build();

app.UseMiddleware((ctx, next) =>
{
    Console.WriteLine($"Middleware before routing, entrypoint is {(ctx.Entrypoint is null ? "null" : "not null")}");

    return next(ctx);
});

app.UseRouting();

app.Map(([Flag(ShortName = 'h', LongName = "help", IsRequired = true)] bool showHelp) => Console.WriteLine("demo [-h|--help]"));
app.Map(() => Console.WriteLine("Hello World!"));
app.MapVersion("[$$$ secret preview version $$$]", displayAppName: true);
app.MapFallback(() => Console.WriteLine("Whaddya want"));

return await app.RunAsync(args);
