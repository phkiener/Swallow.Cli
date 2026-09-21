namespace Swallow.Console.Arguments;

/// <summary>
/// Parse command line arguments in a structured manner.
/// </summary>
public static class Arguments
{
    /// <summary>
    /// Parse the given <paramref name="args"/> into a queryable object.
    /// </summary>
    /// <param name="args">The CLI arguments to parse.</param>
    /// <returns>An object to query the arguments in a structured manner.</returns>
    public static IReadOnlyList<Token> Parse(params IReadOnlyList<string> args)
    {
        return Internal.Tokenizer.Tokenize(args);
    }

    /// <summary>
    /// Parse the given <paramref name="input"/> into a queryable object.
    /// </summary>
    /// <param name="input">The input text to parse.</param>
    /// <returns>An object to query the arguments in a structured manner.</returns>
    /// <remarks>
    /// The input is split on whitespace and then passed into <see cref="Parse(IReadOnlyList{string})"/>.
    /// Whitespace will be ignored if it is inside quotes (<c>"</c> or <c>'</c>). Escape sequences
    /// are supported and will be transformed, i.e. <c>\n</c> will be turned into a newline character.
    /// A backslash-escaped space will not be used to separate the arguments and instead be copied
    /// over verbatim.
    /// </remarks>
    public static IReadOnlyList<Token> Parse(string input)
    {
        var splittedArguments = Internal.Tokenizer.SplitArguments(input);

        return Parse(splittedArguments);
    }

    /// <summary>
    /// Parse the given <paramref name="args"/> into the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to parse the arguments as.</typeparam>
    /// <param name="args">The CLI arguments to parse.</param>
    /// <returns>A parsed instance of <typeparamref name="T"/>.</returns>
    public static T Parse<T>(params string[] args)
    {
        throw new NotImplementedException("This is not implemented. Yet.");
    }
}
