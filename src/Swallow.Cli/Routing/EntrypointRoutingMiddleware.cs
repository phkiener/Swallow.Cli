using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing;

internal sealed class EntrypointRoutingMiddleware(IReadOnlyList<IEntrypoint> entrypoints) : IMiddleware
{
    public Task InvokeAsync(ConsoleContext context, InvocationDelegate next)
    {
        foreach (var entrypoint in entrypoints)
        {
            var flagInfo = entrypoint.RouteParts.OfType<IFlagData>().ToDictionary(static f => f, static _ => false);
            foreach (var flag in flagInfo.Keys)
            {
                // TODO: Support for aliases, Windows-style (/) and multiple casing variants (-> UNIX-style with kebap, Windows-style with Pascal).
                var shortName = flag.ShortName is '\0' ? null : $"-{flag.ShortName}";
                var longName = flag.LongName is null ? null : $"--{flag.LongName}";

                if (shortName is not null && context.Invocation.Arguments.Contains(shortName)
                    || longName is not null && context.Invocation.Arguments.Contains(longName))
                {
                    flagInfo[flag] = true;
                }
            }

            if (flagInfo.All(static kvp => kvp.Value || !kvp.Key.IsRequired))
            {
                context.Entrypoint = entrypoint;

                var enabledFlags = flagInfo.Where(static kvp => kvp.Value).Select(static kvp => kvp.Key);
                context.RouteValues = new RouteValues(enabledFlags);

                break;
            }
        }

        return next(context);
    }
}
