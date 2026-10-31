using System.Reflection;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class BindingContext
{
    internal BindingContext(ParameterInfo parameter) : this(parameter.ParameterType, parameter.GetCustomAttributes(), parameter.HasDefaultValue, parameter.DefaultValue) { }

    internal BindingContext(Type valueType, IEnumerable<Attribute> attributes, bool hasDefaultValue, object? defaultValue)
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
