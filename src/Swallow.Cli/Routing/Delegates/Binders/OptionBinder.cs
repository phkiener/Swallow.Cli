using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class OptionBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        var attribute = bindingContext.Attributes.OfType<OptionAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        if (bindingContext.ValueType == typeof(string))
        {
            boundValue = context.RouteValues?.OptionValues.TryGetValue(attribute, out var value) ?? false ? value : null;
            return true;
        }

        // TODO: BindConverter string -> parameter.ParameterType.

        throw new InvalidOperationException($"{nameof(OptionAttribute)} can only be applied to parameters of type {nameof(String)}");
    }
}
