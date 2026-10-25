using Microsoft.Extensions.DependencyInjection;

namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class FromServiceBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        return TryBindFromServices(bindingContext, context, out boundValue)
               || TryBindFromKeyedServices(bindingContext, context, out boundValue);
    }

    private static bool TryBindFromServices(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        var attribute = bindingContext.Attributes.OfType<FromServicesAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        boundValue = BindFromServiceProvider(bindingContext, context);
        return true;
    }

    private static bool TryBindFromKeyedServices(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        var attribute = bindingContext.Attributes.OfType<FromKeyedServicesAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            boundValue = null;
            return false;
        }

        boundValue = BindFromServiceProvider(bindingContext, context, attribute.Key);
        return true;
    }

    private static object? BindFromServiceProvider(BindingContext bindingContext, ConsoleContext context, object? key = null)
    {
        var resolvedValue = key is null
            ? context.Services.GetKeyedService(bindingContext.ValueType, key)
            : context.Services.GetService(bindingContext.ValueType);

        if (resolvedValue is not null)
        {
            return resolvedValue;
        }

        if (bindingContext.HasDefaultValue)
        {
            return bindingContext.DefaultValue;
        }

        throw new InvalidOperationException($"Cannot resolve {bindingContext.ValueType} from service collection");
    }
}
