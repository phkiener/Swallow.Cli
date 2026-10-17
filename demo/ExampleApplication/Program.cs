using Swallow.Console;
using Swallow.Console.Routing;

var builder = ConsoleApplication.CreateBuilder("demo");
var app = builder.Build();

app.UseMiddleware((ctx, next) =>
{
    Console.WriteLine("Yeet.");
    return next(ctx);
});

app.MapDefault(([Flag(ShortName = 'h', LongName = "help")] bool showHelp) => Console.WriteLine("demo [-h|--help]"));
app.MapDefault(() => Console.WriteLine("Hello World!"));

return await app.RunAsync(args);
