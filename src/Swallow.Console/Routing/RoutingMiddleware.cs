using Swallow.Console.Invocation;

namespace Swallow.Console.Routing;

public sealed class RoutingMiddleware(IReadOnlyCollection<Entrypoint> entrypoints) : IMiddleware
{
    public Task InvokeAsync(InvocationContext invocationContext, InvocationHandler next)
    {
        foreach (var entrypoint in entrypoints)
        {
            var result = entrypoint.RoutePattern.Flags.ToDictionary(static k => k, static _ => false);
            foreach (var flag in result.Keys)
            {
                if (flag.ShortName != 0)
                {
                    if (invocationContext.Arguments.Contains($"-{flag.ShortName}"))
                    {
                        result[flag] = true;
                    }
                }

                if (flag.LongName is not null)
                {
                    if (invocationContext.Arguments.Contains($"--{flag.LongName}"))
                    {
                        result[flag] = true;
                    }
                }

                if (flag.Aliases.Any())
                {
                    if (flag.Aliases.Any(a => invocationContext.Arguments.Contains($"--{a}")))
                    {

                        result[flag] = true;
                    }
                }
            }

            if (result.All(static kvp => kvp.Value || !kvp.Key.IsRequired))
            {
                invocationContext.Entrypoint = entrypoint;
                invocationContext.Flags = result;

                break;
            }
        }

        return next.Invoke(invocationContext);
    }
}
