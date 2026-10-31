using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli;
using Swallow.Cli.Routing;
using TUnit.Assertions.Enums;

namespace Swallow.Console.Test;

public sealed class SmokeTests
{
    [Test]
    public async Task EmptyEntrypoint_CanBeInvoked()
    {
        var entrypointHit = false;

        await using var application = ConsoleApplication.CreateBuilder().Build();
        application.Map(() => { entrypointHit = true; });

        await application.RunAsync([]);

        await Assert.That(entrypointHit).IsTrue();
    }

    [Test]
    public async Task FullyFeaturedEntrypoint_CanBeInvoked()
    {
        var entrypointHit = false;
        var flagSeen = false;
        var optionValue = "";
        var argumentValue = "";
        var injectedValue = "";

        var builder = ConsoleApplication.CreateBuilder();
        builder.Services.AddScoped<string>(static _ => "service");

        await using var application = builder.Build();
        application.Map("foo bar", ([Flag(ShortName = 'f')] bool flag, [Option(LongName = "option")] string option, [Argument(isRequired: true)] string argument, [FromServices] string service) =>
        {
            entrypointHit = true;
            flagSeen = flag;
            optionValue = option;
            argumentValue = argument;
            injectedValue = service;
        });

        await application.RunAsync(["foo", "bar", "-f", "--option", "text", "value"]);

        await Assert.That(entrypointHit).IsTrue();
        await Assert.That(flagSeen).IsTrue();
        await Assert.That(optionValue).IsEqualTo("text");
        await Assert.That(argumentValue).IsEqualTo("value");
        await Assert.That(injectedValue).IsEqualTo("service");
    }

    [Test]
    public async Task Middlewares_AreExecutedInOrder()
    {
        var checkpoints = new List<string>();

        await using var application = ConsoleApplication.CreateBuilder().Build();
        application.UseMiddleware((ctx, next) => { checkpoints.Add("middleware 1"); return next(ctx); });
        application.UseMiddleware((ctx, next) => { checkpoints.Add("middleware 2"); return next(ctx); });
        application.Map(() => checkpoints.Add("entrypoint"));

        await application.RunAsync([]);

        await Assert.That(checkpoints).IsEquivalentTo(["middleware 1", "middleware 2", "entrypoint"], CollectionOrdering.Matching);
    }
}
