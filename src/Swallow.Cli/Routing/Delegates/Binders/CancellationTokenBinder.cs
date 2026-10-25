namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class CancellationTokenBinder : IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue)
    {
        if (bindingContext.ValueType == typeof(CancellationToken))
        {
            boundValue = context.Aborted;
            return true;
        }

        boundValue = null;
        return false;
    }
}
