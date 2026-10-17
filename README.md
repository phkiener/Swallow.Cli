# Swallow.Cli

An experiment.

## Outline

```csharp
var builder = ConsoleApplication.CreateBuilder();
builder.Services.AddStuff();
builder.Services.AddMoreStuff();
builder.Configuration.AddEnvironmentVariables(prefix: "MYAPP_");
builder.Configuration.AddJsonFile("config.json"); // -> in $XDG_CONFIG_HOME

using var app = builder.Build();
app.Map("config", async (ConsoleContext ctx, [Parameter] string key, [Parameter(optional: true)] string? value, [FromServices] IConfiguration configuration) =>
    {
        if (value is null)
        {
            Console.WriteLine(configuration[key]);
            return 0;
        }

        // TOOD: Update setting when value is not null
    });

// see below
app.Map<ConfigCommand>("config");

// common options passed via objects here, but how to access the common options?
// or do i just not care for stuff like "docker compose -f foo/bar.yaml up -d" and nudge it to "docker compose up -d -f foo/bar.yaml"?
app.MapGroup("run", b =>
    {
        b.Map("now", ...);
        b.Map("later", ...);
    });


// TODO: IHostedService-ish thingy?
// TODO: Middlewares? Probably yes - exception handling and stuff.
return app.RunAsync(args);

class ConfigCommandOptions
{
    [Parameter]
    public required string Key { get; set; }

    [Parameter(optional: true)]
    public string? Value { get; set; }
}

class ConfigCommand : ICommand<ConfigCommandOptions>, IAcceptConsoleContext // optional interface to receive the context?
{
    public ConsoleContext Context { get; set; }

    public async Task<int> InvokeAsync(ConfigCommandOptions options, CancellationToken cancellationToken)
    {
        // Do stuff.
        return 0;
    }
}
```
