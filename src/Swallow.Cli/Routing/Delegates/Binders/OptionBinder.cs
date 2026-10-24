using System.Reflection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class OptionBinder : IDelegateParameterBinder
{
    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        var attribute = parameter.GetCustomAttribute<OptionAttribute>();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        if (parameter.ParameterType == typeof(string))
        {
            boundValue = context.RouteValues?.OptionValues.TryGetValue(attribute, out var value) ?? false ? value : null;
            return true;
        }

        // TODO: BindConverter string -> parameter.ParameterType.

        throw new InvalidOperationException($"{nameof(OptionAttribute)} can only be applied to parameters of type {nameof(String)}");
    }
}
