using System.Reflection;
using Swallow.Cli.Abstractions;

namespace Swallow.Cli.Routing.Delegates;

internal interface IDelegateParameterBinder
{
    // TODO: I want something other than ParameterInfo, preferably. But we'll start like this for now.

    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue);
}
