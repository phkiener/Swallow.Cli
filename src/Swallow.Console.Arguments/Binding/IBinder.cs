namespace Swallow.Console.Arguments.Binding;

/// <summary>
/// A binder that can produce a value of a desired type from a given string.
/// </summary>
public interface IBinder
{
    /// <summary>
    /// Whether this binder can produce a value of type <see cref="targetType"/>.
    /// </summary>
    bool CanBind(Type targetType);

    /// <summary>
    /// Try to bind <see cref="value" /> as <see cref="targetType" />,
    /// returning <see langword="true" /> if <see cref="result" /> was successfully bound.
    /// </summary>
    bool TryBind(string value, Type targetType, out object? result);
}
