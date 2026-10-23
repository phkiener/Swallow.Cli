namespace Swallow.Cli.Routing;

[Flags]
internal enum TokenType
{
    Command = 0b0000_0001,
    Flag = 0b0000_0010,
    OptionArgument = 0b0000_0100,
    Argument = 0b0000_1000,
}

internal sealed record CommandlineToken(string Text, TokenType Kind);

internal static class Tokenizer
{
    public static IEnumerable<CommandlineToken> Tokenize(string[] input)
    {
        var seenFlags = false;
        var flagBefore = false;
        var forceArgument = false;

        foreach (var arg in input)
        {
            if (forceArgument)
            {
                yield return new CommandlineToken(arg, TokenType.Argument);

                continue;
            }

            if (arg is "--")
            {
                forceArgument = true;
                flagBefore = false;

                continue;
            }

            if (arg.StartsWith("--"))
            {
                seenFlags = true;
                flagBefore = true;

                yield return new CommandlineToken(arg[2..], TokenType.Flag);
                continue;
            }

            if (arg.StartsWith("-"))
            {
                seenFlags = true;
                flagBefore = true;

                foreach (var flag in arg[1..])
                {
                    yield return new CommandlineToken(flag.ToString(), TokenType.Flag);
                }

                continue;
            }

            if (arg is ['-', var symbol] && char.IsLetterOrDigit(symbol))
            {
                seenFlags = true;
                flagBefore = true;

                yield return new CommandlineToken(arg[1..], TokenType.Flag);
                continue;
            }

            var kind = TokenType.Argument;
            if (flagBefore)
            {
                kind |= TokenType.OptionArgument;
            }

            if (!seenFlags)
            {
                kind |= TokenType.Command;
            }

            flagBefore = false;
            yield return new CommandlineToken(arg, kind);
        }
    }
}
