using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal abstract class ParameterBinderBase<TIn> : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        if (!TryGetSourceValue(bindingContext, context, out var sourceValue))
        {
            boundValue = null;
            return false;
        }

        // TODO: "Accepts null" is relevant here.
        if (sourceValue is null)
        {
            boundValue = null;
            return true;
        }

        if (bindingContext.ValueType == typeof(TIn))
        {
            boundValue = sourceValue;
            return true;
        }

        var targetDescriptor = TypeDescriptor.GetConverter(bindingContext.ValueType);
        if (targetDescriptor.CanConvertFrom(typeof(TIn)))
        {
            boundValue = targetDescriptor.ConvertFrom(sourceValue);
            return true;
        }

        throw new InvalidOperationException($"Cannot convert from {typeof(TIn).Name} to {bindingContext.ValueType.Name}.");
    }

    protected abstract bool TryGetSourceValue(BindingContext bindingContext, ConsoleContext context, [MaybeNullWhen(false)] out TIn value);
}
