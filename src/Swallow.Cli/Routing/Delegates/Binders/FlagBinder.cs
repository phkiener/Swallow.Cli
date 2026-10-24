using System.Reflection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class FlagBinder : IDelegateParameterBinder
{
    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        var attribute = parameter.GetCustomAttribute<FlagAttribute>();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        if (parameter.ParameterType == typeof(bool) || parameter.ParameterType == typeof(bool?))
        {
            boundValue = context.RouteValues?.EnabledFlags.Contains(attribute);
            return true;
        }

        // TODO: BindConverter bool -> parameter.ParameterType.

        throw new InvalidOperationException($"{nameof(FlagAttribute)} can only be applied to parameters of type {nameof(Boolean)}");
    }
}
