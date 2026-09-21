namespace Swallow.Console.Arguments;

/// <summary>
/// A single token passed from commandline.
/// </summary>
/// <param name="Type">The type of token.</param>
/// <param name="Value">The parsed value.</param>
public readonly record struct Token(TokenType Type, string Value)
{
    /// <summary>
    /// An long option declared using multiple characters.
    /// </summary>
    /// <param name="name">The parsed option.</param>
    public static Token Option(string name) => new(TokenType.Option, name);

    /// <summary>
    /// An argument that is unambiguously used as value for a preceding <see cref="Token.Option(string)"/>.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token OptionValue(string value) => new(TokenType.OptionValue, value);

    /// <summary>
    /// An argument that is unambiguously used as a standalone parameter.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token Parameter(string value) => new(TokenType.Parameter, value);

    /// <summary>
    /// /// An argument that - depending on context - can be used as value for an option
    /// (<see cref="Option(string)"/>) <em>or</em> as standalone parameter (<see cref="Parameter"/>). No
    /// differentiation can be made based on syntax alone.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token ParameterOrOptionValue(string value) => new(TokenType.ParameterOrOptionValue, value);
}
