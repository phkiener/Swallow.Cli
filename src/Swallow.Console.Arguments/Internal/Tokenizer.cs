using System.Text.RegularExpressions;

namespace Swallow.Console.Arguments.Internal;

internal static partial class Tokenizer
{
    public static List<string> SplitArguments(string input)
    {
        var arguments = new List<string>();

        var argumentStart = 0;
        char? quotingCharacter = null;
        var isEscaped = false;

        for (var i = 0; i < input.Length; i++)
        {
            if (isEscaped)
            {
                isEscaped = false;
                continue;
            }

            if (input[i] == '\\')
            {
                isEscaped = true;
                continue;
            }

            if (quotingCharacter is not null)
            {
                if (input[i] == quotingCharacter)
                {
                    var argument = input[argumentStart..i];
                    arguments.Add(Unescape(argument));

                    quotingCharacter = null;
                    argumentStart = i + 1;
                }

                continue;
            }

            if (input[i] is '"' or '\'')
            {
                quotingCharacter = input[i];
                argumentStart = i + 1;
            }

            if (char.IsWhiteSpace(input[i]))
            {
                if (i - argumentStart >= 1)
                {
                    var argument = input[argumentStart..i];
                    arguments.Add(Unescape(argument));
                }

                argumentStart = i + 1;
            }
        }

        if (argumentStart < input.Length)
        {
            var argument = input[argumentStart..];
            arguments.Add(Unescape(argument));
        }

        return arguments;
    }

    public static List<Token> Tokenize(IReadOnlyList<string> arguments)
    {
        var tokens = new List<Token>(arguments.Count);

        var canBeOptionValue = false;
        var forceParameter = false;
        foreach (var argument in arguments)
        {
            if (argument is "--")
            {
                forceParameter = true;
                continue;
            }

            if (forceParameter)
            {
                tokens.Add(Token.Parameter(argument));
                continue;
            }

            var unixStyleMatch = UnixStyleFlag().Match(argument);
            if (unixStyleMatch.Success)
            {
                var shortOption = unixStyleMatch.Groups["shortoption"];
                if (shortOption.Success)
                {
                    var options = shortOption.Captures.Select(static c => Token.Option(c.Value));
                    tokens.AddRange(options);

                    canBeOptionValue = true;
                    continue;
                }

                var longOption = unixStyleMatch.Groups["longoption"];
                if (longOption.Success)
                {
                    tokens.Add(Token.Option(longOption.Value));

                    var optionValue = unixStyleMatch.Groups["optionvalue"];
                    if (optionValue.Success)
                    {
                        tokens.Add(Token.OptionValue(optionValue.Value));
                        canBeOptionValue = false;
                    }
                    else
                    {
                        canBeOptionValue = true;
                    }

                    continue;
                }
            }

            var windowsStyleMatch = WindowsStyleFlag().Match(argument);
            if (windowsStyleMatch.Success)
            {
                var option = windowsStyleMatch.Groups["option"];
                if (option.Success)
                {
                    tokens.Add(Token.Option(option.Value));

                    var optionValue = windowsStyleMatch.Groups["optionvalue"];
                    if (optionValue.Success)
                    {
                        tokens.Add(Token.OptionValue(optionValue.Value));
                        canBeOptionValue = false;
                    }
                    else
                    {
                        canBeOptionValue = true;
                    }
                }

                continue;
            }

            var value = canBeOptionValue
                ? Token.ParameterOrOptionValue(argument)
                : Token.Parameter(argument);

            tokens.Add(value);
            canBeOptionValue = false;
        }

        return tokens;
    }

    private static string Unescape(string input)
    {
        return input
            .Replace(@"\n", "\n")
            .Replace(@"\r", "\r")
            .Replace(@"\t", "\t")
            .Replace(@"\ ", " ")
            .Replace(@"\\", "\\")
            .Replace(@"\""", "\"")
            .Replace(@"\'", "\'");
    }

    [GeneratedRegex(@"^(-(?<shortoption>\w)+|--(?<longoption>\w+)(=(?<optionvalue>.+))?)$")]
    private static partial Regex UnixStyleFlag();

    [GeneratedRegex(@"^/(?<option>\w+)(:(?<optionvalue>.+))?$")]
    private static partial Regex WindowsStyleFlag();
}
