using System.Reflection;
using Swallow.Cli.Abstractions;

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

    public Type ValueType { get; }
    public IReadOnlyCollection<Attribute> Attributes { get; }
    public bool HasDefaultValue { get; }
    public object? DefaultValue { get; }
}

internal interface IDelegateParameterBinder
{
    public bool TryBind(BindingContext bindingContext, ConsoleContext context, out object? boundValue);
}
