using System.ComponentModel;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class ArgumentBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        var attribute = bindingContext.Attributes.OfType<ArgumentAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        var rawValue = context.RouteValues?.Arguments.TryGetValue(attribute, out var value) ?? false ? value : null;
        if (bindingContext.ValueType == typeof(string))
        {
            boundValue = rawValue;
            return true;
        }

        var converter = TypeDescriptor.GetConverter(bindingContext.ValueType);
        if (converter.CanConvertFrom(typeof(string)) && rawValue is not null)
        {
            boundValue = converter.ConvertFrom(rawValue);
            return true;
        }

        boundValue = null;
        return false;
    }
}
