using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class FromServiceBinder : IDelegateParameterBinder
{
    public bool TryBind(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        return TryBindFromServices(parameter, context, out boundValue)
               || TryBindFromKeyedServices(parameter, context, out boundValue);
    }

    private static bool TryBindFromServices(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        var attribute = parameter.GetCustomAttribute<FromServicesAttribute>();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        boundValue = BindFromServiceProvider(parameter, context);
        return true;
    }

    private static bool TryBindFromKeyedServices(ParameterInfo parameter, ConsoleContext context, out object? boundValue)
    {
        var attribute = parameter.GetCustomAttribute<FromKeyedServicesAttribute>();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        boundValue = BindFromServiceProvider(parameter, context, attribute.Key);
        return true;
    }

    private static object? BindFromServiceProvider(ParameterInfo parameter, ConsoleContext context, object? key = null)
    {
        var resolvedValue = key is null
            ? context.Services.GetKeyedService(parameter.ParameterType, key)
            : context.Services.GetService(parameter.ParameterType);

        if (resolvedValue is not null)
        {
            return resolvedValue;
        }

        if (parameter.HasDefaultValue)
        {
            return parameter.DefaultValue;
        }

        throw new InvalidOperationException($"Cannot resolve {parameter.ParameterType} from service collection");
    }
}
