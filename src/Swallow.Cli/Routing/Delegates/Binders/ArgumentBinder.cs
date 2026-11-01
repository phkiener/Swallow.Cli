namespace Swallow.Cli.Routing.Delegates.Binders;

internal sealed class ArgumentBinder : ParameterBinderBase<string?>
{
    protected override bool TryGetSourceValue(BindingContext bindingContext, ConsoleContext context, out string? value)
    {
        var attribute = bindingContext.Attributes.OfType<ArgumentAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            value = null;
            return false;
        }

        if (context.RouteValues.Arguments.TryGetValue(attribute, out value))
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
