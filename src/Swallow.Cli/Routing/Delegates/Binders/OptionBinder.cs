namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class OptionBinder : ParameterBinderBase<string?>
{
    protected override bool TryGetSourceValue(BindingContext bindingContext, ConsoleContext context, out string? value)
    {
        var attribute = bindingContext.Attributes.OfType<OptionAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            value = null;
            return false;
        }

        if (context.RouteValues.TryGetValue(attribute, out value))
        {
            return true;
        }

        if (attribute.IsRequired)
        {
            throw new InvalidOperationException($"Required option {attribute} is not set.");
        }

        value = null;
        return true;
    }
}
