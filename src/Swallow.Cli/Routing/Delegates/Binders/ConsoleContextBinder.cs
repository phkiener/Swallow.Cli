namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class ConsoleContextBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        if (bindingContext.ValueType == typeof(ConsoleContext))
        {
            boundValue = context;
            return true;
        }

        boundValue = null;
        return false;
    }
}
