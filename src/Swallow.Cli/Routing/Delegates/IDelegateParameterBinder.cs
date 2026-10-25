using System.Reflection;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class BindingContext
{
    internal BindingContext(ParameterInfo parameter)
    {
        ValueType = parameter.ParameterType;
        HasDefaultValue = parameter.HasDefaultValue;
        DefaultValue = parameter.DefaultValue;
        Attributes = parameter.GetCustomAttributes().ToArray();
    }

    internal BindingContext(Type valueType, IReadOnlyCollection<Attribute> attributes, bool hasDefaultValue, object? defaultValue)
    {
        ValueType = valueType;
        HasDefaultValue = hasDefaultValue;
        DefaultValue = defaultValue;
        Attributes = attributes.ToArray();
    }

    public Type ValueType { get; }
    public IReadOnlyCollection<Attribute> Attributes { get; }
    public bool HasDefaultValue { get; }
    public object? DefaultValue { get; }
}

internal interface IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue);
}
