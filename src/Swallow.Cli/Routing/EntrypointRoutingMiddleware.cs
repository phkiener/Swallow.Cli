using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing;

internal sealed class EntrypointRoutingMiddleware(IReadOnlyList<IEntrypoint> entrypoints) : IMiddleware
{
    public Task InvokeAsync(ConsoleContext context, InvocationDelegate next)
    {
        var tokens = Tokenizer.Tokenize(context.Invocation.Arguments).ToArray();

        foreach (var entrypoint in entrypoints)
        {
            if (IsMatch(tokens, entrypoint.RouteParts, out var routeValues))
            {
                context.Entrypoint = entrypoint;
                context.RouteValues = routeValues;

                break;
            }
        }

        return next(context);
    }

    private static bool IsMatch(CommandlineToken[] tokens, IRoutePart[] routeParts, out RouteValues routeValues)
    {
        var tokenIndex = 0;

        // First step: Match the prefix. This part is determined by the route itself.
        foreach (var prefix in routeParts.OfType<ICommandData>())
        {
            ref var token = ref tokens[tokenIndex];
            if (!token.Kind.HasFlag(TokenType.Command) || token.Text != prefix.Name)
            {
                routeValues = RouteValues.Empty;
                return false;
            }

            tokenIndex++;
        }

        var flagResults = routeParts.OfType<IFlagData>().ToDictionary(static f => f, static _ => false);
        var flagByShortName = flagResults.Keys
            .Where(static f => f.ShortName != '\0')
            .ToDictionary(static f => f.ShortName, static f => f);

        var flagByLongName = flagResults.Keys
            .Where(static f => f.LongName is not null)
            .ToDictionary(static f => f.LongName!, static f => f);


        // Next step: Match the tokens to the remaining parts. Just assume that there's no overlap between flags, first one wins.
        for (; tokenIndex < tokens.Length; ++tokenIndex)
        {
            ref var token = ref tokens[tokenIndex];

            if (token.Kind.HasFlag(TokenType.Flag))
            {
                if ((token.Text is [var symbol] && flagByShortName.TryGetValue(symbol, out var matchingFlag))
                    || flagByLongName.TryGetValue(token.Text, out matchingFlag))
                {
                    flagResults[matchingFlag] = true;
                    continue;
                }
            }

            throw new NotImplementedException($"Token type {token.Kind} not implemented yet");
        }

        if (flagResults.All(static kvp => kvp.Value || !kvp.Key.IsRequired))
        {
            var enabledFlags = flagResults.Where(static kvp => kvp.Value).Select(static kvp => kvp.Key);

            routeValues = new RouteValues(enabledFlags);
            return true;
        }


        routeValues = RouteValues.Empty;
        return false;
    }
}
