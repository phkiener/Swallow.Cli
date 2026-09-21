namespace Swallow.Console.Arguments;

/// <summary>
/// The type of a <see cref="Token"/>.
/// </summary>
[Flags]
public enum TokenType
{
    /// <summary>
    /// An option (or flag), i.e. <c>--bar</c>.
    /// </summary>
    Option = 0b0000_0001,

    /// <summary>
    /// A value that is passed to an option.
    /// </summary>
    OptionValue = 0b0000_0010,

    /// <summary>
    /// A parameter.
    /// </summary>
    Parameter = 0b0010_0000,

    /// <summary>
    /// A value that can be either a <see cref="Parameter"/> or an
    /// <see cref="OptionValue"/>, depending on context.
    /// </summary>
    ParameterOrOptionValue = OptionValue | Parameter
}
