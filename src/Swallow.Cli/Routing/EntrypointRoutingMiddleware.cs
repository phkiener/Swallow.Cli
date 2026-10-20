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

        var optionResults = routeParts.OfType<IOptionData>().ToDictionary(static f => f, static string? (_) => null);
        var optionByShortName = optionResults.Keys
            .Where(static f => f.ShortName != '\0')
            .ToDictionary(static f => f.ShortName, static f => f);

        var optionByLongName = optionResults.Keys
            .Where(static f => f.LongName is not null)
            .ToDictionary(static f => f.LongName!, static f => f);

        var arguments = routeParts.OfType<IArgumentData>().Select(static arg => new MatchedArgument(arg)).ToList();

        // Next step: Match the tokens to the remaining parts. Just assume that there's no overlap between flags, first one wins.
        for (; tokenIndex < tokens.Length; ++tokenIndex)
        {
            ref var token = ref tokens[tokenIndex];

            if (token.Kind.HasFlag(TokenType.Flag))
            {
                if ((token.Text is [var flagSymbol] && flagByShortName.TryGetValue(flagSymbol, out var matchingFlag))
                    || flagByLongName.TryGetValue(token.Text, out matchingFlag))
                {
                    flagResults[matchingFlag] = true;
                    continue;
                }

                if ((token.Text is [var optionSymbol] && optionByShortName.TryGetValue(optionSymbol, out var matchingOption))
                    || optionByLongName.TryGetValue(token.Text, out matchingOption))
                {
                    if (tokenIndex + 1 < tokens.Length)
                    {
                        ref var nextToken = ref tokens[tokenIndex + 1];
                        if (nextToken.Kind.HasFlag(TokenType.OptionArgument))
                        {
                            optionResults[matchingOption] = nextToken.Text;
                            tokenIndex += 1;

                            continue;
                        }
                    }
                }
            }

            if (token.Kind.HasFlag(TokenType.Argument))
            {
                var availableArgument = arguments.FirstOrDefault(arg => arg.Value is null);
                if (availableArgument is not null)
                {
                    availableArgument.Value = token.Text;
                    continue;
                }
            }

            // Token cannot be matched. Goodbye.
            routeValues = RouteValues.Empty;
            return false;
        }

        if (flagResults.Any(static kvp => kvp.Key.IsRequired && !kvp.Value))
        {
            routeValues = RouteValues.Empty;
            return false;
        }

        if (optionResults.Any(static kvp => kvp.Key.IsRequired && kvp.Value is null))
        {
            routeValues = RouteValues.Empty;
            return false;
        }

        if (arguments.Any(static arg => arg.Argument.IsRequired && arg.Value is null))
        {
            routeValues = RouteValues.Empty;
            return false;
        }

        var enabledFlags = flagResults.Where(static kvp => kvp.Value).Select(static kvp => kvp.Key);
        var optionValues = optionResults.Where(static kvp => kvp.Value is not null).ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value!);
        var argumentValues = arguments.Where(static arg => arg.Value is not null).ToDictionary(static arg => arg.Argument, static arg => arg.Value!);

        routeValues = new RouteValues(enabledFlags, optionValues, argumentValues);
        return true;
    }

    private sealed class MatchedArgument(IArgumentData argument)
    {
        public IArgumentData Argument { get; } = argument;
        public string? Value { get; set; } = null;
    }
}
