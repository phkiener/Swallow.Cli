using Swallow.Cli.Abstractions;
using Swallow.Cli.Routing.Delegates.Binders;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateInvoker
{
    private static readonly List<IDelegateParameterBinder> binders =
    [
        new ConsoleContextBinder(),
        new CancellationTokenBinder(),
        new FromServiceBinder(),
        new FlagBinder(),
        new OptionBinder(),
        new ArgumentBinder()
    ];

    public Task InvokeDelegate(Delegate @delegate, ConsoleContext context)
    {
        var parameters = @delegate.Method.GetParameters();
        var arguments = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var bindingSucceeded = false;

            foreach (var binder in binders)
            {
                if (binder.TryBind(parameter, context, out var value))
                {
                    arguments[i] = value;
                    bindingSucceeded = true;

                    break;
                }
            }

            if (!bindingSucceeded)
            {

                throw new InvalidOperationException($"Parameter of type {parameter.ParameterType} cannot be bound for delegate in {nameof(DelegateEntrypoint)}.");
            }
        }

        var result = @delegate.DynamicInvoke(arguments);
        return result as Task ?? Task.CompletedTask;
    }
}
