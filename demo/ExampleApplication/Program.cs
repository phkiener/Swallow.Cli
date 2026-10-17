using Swallow.Console.Host;
using Swallow.Console.Host.Routing;

var builder = ConsoleApplication.CreateBuilder("demo");
var app = builder.Build();

app.MapDefault(([Flag(ShortName = 'h', LongName = "help")] bool showHelp) => Console.WriteLine("demo [-h|--help]"));
app.MapDefault(() => Console.WriteLine("Hello World!"));

return await app.RunAsync(args);
