namespace Swallow.Console.Arguments.Binding;

/// <summary>
/// A no-op binder for <see cref="string" />s.
/// </summary>
public sealed class StringBinder : IBinder
{
    /// <inheritdoc />
    public bool CanBind(Type targetType) => targetType == typeof(string);

    /// <inheritdoc />
    public bool TryBind(string value, Type targetType, out object? result)
    {
        result = value;
        return true;
    }
}
