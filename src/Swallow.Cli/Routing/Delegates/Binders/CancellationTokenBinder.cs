using System.Reflection;
using Swallow.Cli.Abstractions;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class CancellationTokenBinder : IDelegateParameterBinder
{
    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        if (parameter.ParameterType == typeof(CancellationToken))
        {
            boundValue = context.Aborted;
            return true;
        }

        boundValue = null;
        return false;
    }
}
