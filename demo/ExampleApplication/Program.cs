using Swallow.Cli;
using Swallow.Cli.Routing;

var builder = ConsoleApplication.CreateBuilder();
var app = builder.Build();

app.UseMiddleware((ctx, next) =>
{
    Console.WriteLine("Yeet.");
    return next(ctx);
});

app.Map(([Flag(ShortName = 'h', LongName = "help", IsRequired = true)] bool showHelp) => Console.WriteLine("demo [-h|--help]"));
app.Map(() => Console.WriteLine("Hello World!"));

return await app.RunAsync(args);
