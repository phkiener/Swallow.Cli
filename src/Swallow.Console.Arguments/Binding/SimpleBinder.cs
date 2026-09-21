namespace Swallow.Console.Arguments.Binding;

/// <summary>
/// Generic binder that adapts a <em>TryParse</em> method like <see cref="int.TryParse(string, out int)" />.
/// </summary>
public sealed class SimpleBinder<T>(SimpleBinder<T>.TryParse parser) : IBinder where T : struct
{
    /// <summary>
    /// Delegate for a <em>TryParse</em> method.
    /// </summary>
    public delegate bool TryParse(string text, out T value);

    /// <inheritdoc />
    public bool CanBind(Type targetType) => targetType == typeof(T) || targetType == typeof(T?);

    /// <inheritdoc />
    public bool TryBind(string value, Type targetType, out object? result)
    {
        if (string.IsNullOrWhiteSpace(value) && targetType == typeof(T?))
        {
            result = null;
            return true;
        }

        if (parser.Invoke(value, out var parsed))
        {
            result = parsed;
            return true;
        }

        result = null;
        return false;
    }
}
