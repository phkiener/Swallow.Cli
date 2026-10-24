using System.Reflection;
using Swallow.Cli.Abstractions;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class ConsoleContextBinder : IDelegateParameterBinder
{
    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        if (parameter.ParameterType == typeof(ConsoleContext))
        {
            boundValue = context;
            return true;
        }

        boundValue = null;
        return false;
    }
}
