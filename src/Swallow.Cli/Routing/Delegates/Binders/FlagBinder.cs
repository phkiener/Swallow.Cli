namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class FlagBinder : ParameterBinderBase<bool>
{
    protected override bool TryGetSourceValue(BindingContext bindingContext, ConsoleContext context, out bool value)
    {
        var attribute = bindingContext.Attributes.OfType<FlagAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            value = false;
            return false;
        }

        value = context.RouteValues.ContainsKey(attribute);
        if (!value && attribute.IsRequired)
        {
            throw new InvalidOperationException($"Required flag {attribute} is not enabled.");
        }

        return true;
    }
}
