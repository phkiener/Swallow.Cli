using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class FlagBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        var attribute = bindingContext.Attributes.OfType<FlagAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        if (bindingContext.ValueType == typeof(bool) || bindingContext.ValueType == typeof(bool?))
        {
            boundValue = context.RouteValues?.EnabledFlags.Contains(attribute);
            return true;
        }

        // TODO: BindConverter bool -> parameter.ParameterType.

        throw new InvalidOperationException($"{nameof(FlagAttribute)} can only be applied to parameters of type {nameof(Boolean)}");
    }
}
